using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;

namespace mmcSolutions.SolrParser
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultParser
    {
        /// <summary>
        /// Xpath string to Solr document.
        /// </summary>
        internal static string XpathSolrDocuments { get { return "response/result/doc"; } }
        /// <summary>
        /// Get a T instance with its properties filled based on a object.
        /// </summary>
        /// <typeparam name="T">The T instace to be returned.</typeparam>
        /// <param name="obj">A object with properties that will be used to fill the instance.</param>
        /// <returns>A instance of T class with properties with same name and datatype of passed object filled.</returns>
        internal static T GetInstanceWithPropertiesFilled<T>(object obj)
        {
            var instance = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                var epi = obj.GetType().GetProperty(pi.Name);
                if (epi != null && pi.PropertyType.Equals(epi.PropertyType))
                    pi.SetValue(instance, epi.GetValue(obj, null), null);
            }
            return instance;
        }

        /// <summary>
        /// Cast a XML Solr document in a generic type object.
        /// </summary>
        /// <param name="xml">The Solr XML result of a query.</param>
        /// <returns>A IDocument with its properties filled according to XML data.</returns>
        public static IDocument Parse(String xml)
        {
            return Parse<Document>(xml);
        }
        /// <summary>
        /// Cast a XML Solr document in a generic type object.
        /// </summary>
        /// <typeparam name="T">A generic type to be returned.</typeparam>
        /// <param name="xml">The Solr XML result of a query.</param>
        /// <returns>A object with its properties filled according to XML data.</returns>
        public static T Parse<T>(String xml)
        {
            var instance = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                try
                {
                    var attr = GetAttribute<ISolrAttribute>(pi);
                    if (attr != null && !attr.Type.Equals(SolrType.NotConvertible))
                        pi.SetValue(instance, GetObjectValue(pi, attr, xml), null);
                }
                finally { }
            }
            return instance;
        }
        /// <summary>
        /// Returns a array according to XML query result data.
        /// </summary>
        /// <typeparam name="T">Specify the type of array to be returned.</typeparam>
        /// <param name="xml">The Solr XML result of a query.</param>
        /// <param name="name">Name of element on Solr XML result.</param>
        /// <returns>A T list of each document found on Solr XML result.</returns>
        public static List<T> GetArray<T>(string xml, string name)
        {
            var result = new List<T>();
            var element = GetXElementFromXML(xml, SolrType.Array, name);

            if (element != null)
            {
                var nodeName = GetNodeName<T>();
                foreach (XNode obj in element.Nodes())
                {
                    try { result.Add((T)Convert.ChangeType(obj.RemoveNodeName(nodeName), typeof(T))); }
                    catch { }
                }
            }
            return result;
        }
        /// <summary>
        /// Get a XML node list of "response/result/doc" elements from a XML Solr result.
        /// </summary>
        /// <param name="xml">The Solr XML result of a query.</param>
        /// <returns>A XmlNodeList with Solr docs.</returns>
        public static System.Xml.XmlNodeList GetSolrDocumentsFromXML(string xml)
        {
            var docs = new System.Xml.XmlDocument();
            docs.LoadXml(xml);
            return docs.SelectNodes(XpathSolrDocuments);
        }

        /// <summary>
        /// Get the Solr node name according a specific framework type.
        /// </summary>
        /// <typeparam name="T">Specify the framework type of Solr type name to be returned.</typeparam>
        /// <param name="instance">A instance</param>
        /// <returns></returns>
        private static string GetNodeName<T>()
        {
            var instance = typeof(T);

            if (instance.Equals(typeof(string)))
                return "str";
            if (instance.Equals(typeof(short)))
                return "short";
            if (instance.Equals(typeof(int)))
                return "int";
            if (instance.Equals(typeof(long)))
                return "long";
            if (instance.Equals(typeof(DateTime)))
                return "date";
            if (instance.Equals(typeof(bool)))
                return "bool";
            if (instance.Equals(typeof(decimal)))
                return "decimal";
            if (instance.Equals(typeof(double)))
                return "double";
            if (instance.Equals(typeof(float)))
                return "float";

            return null;
        }

        /// <summary>
        /// Get the value of a object based on a property in a Solr XML query result.
        /// </summary>
        /// <param name="pi">A PropertyInfo with a SolrAttribute used to select data in XML.</param>
        /// <param name="xml">A Solr XML query result.</param>
        /// <param name="prefix">On complex objects it contains the prefix to access an value data.</param>
        /// <returns></returns>
        private static object GetObjectValue(PropertyInfo pi, ISolrAttribute attribute, string xml, string prefix = "")
        {
            var element = XDocument.Parse(xml).Descendants("doc");
            var attr = new SolrAttribute { Type = attribute.Type, IsNullable = attribute.IsNullable };

            if (attribute is SolrAttribute)
                attr.Name = ((SolrAttribute)attribute).Name;

            if (!string.IsNullOrEmpty(prefix))
                attr.Name = string.Format("{0}{1}", prefix, attr.Name);

            switch (attribute.Type)
            {
                case SolrType.Array:
                    return GetArray(pi, element, attr.Name);

                case SolrType.Date:
                    return GetDateObjectValue(element, attr);

                case SolrType.Bool:
                    return GetBooleanObjectValue(element, attr);

                case SolrType.Decimal:
                case SolrType.Currency:
                    return GetDecimalObjectValue(element, attr);

                case SolrType.Double:
                    return GetDoubleObjectValue(element, attr);

                case SolrType.Int:
                    return GetIntObjectValue(element, attr);

                case SolrType.Long:
                    return GetLongObjectValue(element, attr);

                case SolrType.Short:
                    return GetShortObjectValue(element, attr);

                case SolrType.Float:
                    return GetFloatObjectValue(element, attr);

                case SolrType.String:
                case SolrType.Binary:
                    return element.Select(e => new { Value = e.ToString(attr.Name) }).SingleOrDefault().Value;

                case SolrType.Complex:
                    return GetComplexObjectValue(pi, prefix, element);

            }

            return null;
        }

        #region Auxiliar methods to GetObjectValue(pi, attribute, xml, prefix) method
        /// <summary>
        /// Get a date value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>.
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetDateObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToDateOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToDate(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a boolean value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetBooleanObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToBooleanOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToBoolean(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a decimal value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetDecimalObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToDecimalOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToDecimal(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a double value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetDoubleObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToDoubleOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToDouble(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a int value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetIntObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToIntOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToInt(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a long value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetLongObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToLongOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToLong(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a short value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetShortObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToShortOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToShort(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a float value of a object based on a <see cref="IEnumerable<XElement> element"/> and a <see cref="SolrAttribute"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetFloatObjectValue(IEnumerable<XElement> element, SolrAttribute attr)
        {
            if (attr.IsNullable)
                return element.Select(e => new { Value = e.ToFloatOrNull(attr.Name) }).SingleOrDefault().Value;
            else
                return element.Select(e => new { Value = e.ToFloat(attr.Name) }).SingleOrDefault().Value;
        }
        /// <summary>
        /// Get a complex object based on a <see cref="IEnumerable<XElement> element"/>, a <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="element">A <see cref="IEnumerable<XElement> element"/> that contains a <see cref="SolrAttribute"/></param>
        /// <param name="attr">A <see cref="SolrAttribute"/> used to identify the date information on <see cref="element"/></param>
        /// <returns></returns>
        private static object GetComplexObjectValue(PropertyInfo pi, string prefix, IEnumerable<XElement> element)
        {
            var sca = GetAttribute<SolrComplexAttribute>(pi);
            prefix += string.Format("{0}{1}", sca.Prefix, sca.Separator);
            return element.Select(e => new { Value = e.ToComplex(pi, prefix) }).SingleOrDefault().Value;
        }
        #endregion

        private static object GetArray(PropertyInfo pi, IEnumerable<XElement> element, string attributeName)
        {
            if (pi.PropertyType.Equals(typeof(List<string>)))
                return element.Select(e => new { Value = e.ToArray<string>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<int>)))
                return element.Select(e => new { Value = e.ToArray<int>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<short>)))
                return element.Select(e => new { Value = e.ToArray<short>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<long>)))
                return element.Select(e => new { Value = e.ToArray<long>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<decimal>)))
                return element.Select(e => new { Value = e.ToArray<decimal>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<double>)))
                return element.Select(e => new { Value = e.ToArray<double>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<float>)))
                return element.Select(e => new { Value = e.ToArray<float>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<DateTime>)))
                return element.Select(e => new { Value = e.ToArray<bool>(attributeName) }).SingleOrDefault().Value;
            if (pi.PropertyType.Equals(typeof(List<DateTime>)))
                return element.Select(e => new { Value = e.ToArray<bool>(attributeName) }).SingleOrDefault().Value;
            return null;
        }
        /// <summary>
        /// Get a specifit attribute type in a PropertyInfo.
        /// </summary>
        /// <typeparam name="T">The attribute type to be returned.</typeparam>
        /// <param name="property">A PropertyInfo that could contain a T attribute.</param>
        /// <returns></returns>
        private static T GetAttribute<T>(PropertyInfo property)
        {
            foreach (var attr in property.GetCustomAttributes(typeof(T), false))
                if (attr != null && attr is T)
                    return (T)attr;
            return default(T);
        }
        /// <summary>
        /// Get a XElement from a Sorl XML according to a Solr data type.
        /// </summary>
        /// <param name="xml">A XML query result.</param>
        /// <param name="type">A Solr type of data to be returned.</param>
        /// <param name="name">Name of element to be returned.</param>
        /// <returns></returns>
        private static XElement GetXElementFromXML(string xml, SolrType type, string name)
        {
            var doc = XDocument.Parse(xml).Descendants("doc");
            try
            {
                switch (type)
                {
                    case SolrType.Array:
                        return doc.Elements("arr").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Date:
                        return doc.Elements("date").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Bool:
                        return doc.Elements("bool").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Decimal:
                        return doc.Elements("decimal").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Double:
                        return doc.Elements("double").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Int:
                        return doc.Elements("int").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Long:
                        return doc.Elements("long").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Short:
                        return doc.Elements("short").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.Float:
                        return doc.Elements("float").First(s => s.Attribute("name").Value.Equals(name));

                    case SolrType.String:
                        return doc.Elements("string").First(s => s.Attribute("name").Value.Equals(name));
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Cast a XML Solr document in a generic type object.
        /// </summary>
        /// <param name="xml">The Solr XML result of a query.</param>
        /// <param name="info">A PropertyInfo with properties to be casted.</param>
        /// <param name="prefix">A initial text to be added as prefix to get fields values.</param>
        /// <returns>A object with its properties filled according to XML data.</returns>
        public static object Parse(String xml, PropertyInfo info, string prefix = "")
        {
            var instance = Activator.CreateInstance(info.PropertyType);
            var properties = info.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (string.IsNullOrEmpty(prefix))
                prefix = GetAttribute<SolrComplexAttribute>(info).Prefix;
            foreach (PropertyInfo pi in properties)
            {
                try
                {
                    var attr = GetAttribute<ISolrAttribute>(pi);
                    if (!attr.Type.Equals(SolrType.NotConvertible))
                        pi.SetValue(instance, GetObjectValue(pi, attr, xml, prefix), null);
                }
                finally { }
            }
            return instance;
        }

    }
}
