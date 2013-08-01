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
            var entity = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                var epi = obj.GetType().GetProperty(pi.Name);
                if (epi != null && pi.PropertyType.Equals(epi.PropertyType))
                    pi.SetValue(entity, epi.GetValue(obj, null), null);
            }
            return entity;
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
            var entity = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                try
                {
                    if (!string.IsNullOrEmpty(GetAttribute<SolrAttribute>(pi).Name)) 
                        pi.SetValue(entity, GetObjectValue(pi, xml), null);
                }
                finally { }
            }
            return entity;
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
            var nodeName = GetNodeName<T>();
            var element = GetXElementFromXML(xml, SolrType.Array, name);
            if (element != null)
                foreach (XNode obj in element.Nodes())
                result.Add((T)Convert.ChangeType(obj.RemoveNodeName(nodeName), typeof(T)));
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
            if (instance.Equals(typeof(string)))
                return "str";

            return null;
        }
        /// <summary>
        /// Get the value of a object based on a property in a Solr XML query result.
        /// </summary>
        /// <param name="pi">A PropertyInfo with a SolrAttribute used to select data in XML.</param>
        /// <param name="xml">A Solr XML query result.</param>
        /// <returns></returns>
        private static object GetObjectValue(PropertyInfo pi, string xml)
        {
            var element = XDocument.Parse(xml).Descendants("doc");
            var attribute = GetAttribute<SolrAttribute>(pi);
            
            switch (attribute.Type)
            {
                case SolrType.Array:
                    return element.Select(e => new { Value = e.ToArray(attribute.Name) }).ToList()[0].Value;

                case SolrType.Date:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToDateOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToDate(attribute.Name) }).ToList()[0].Value;

                case SolrType.Bool:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToBooleanOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToBoolean(attribute.Name) }).ToList()[0].Value;

                case SolrType.Decimal:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToDecimalOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToDecimal(attribute.Name) }).ToList()[0].Value;

                case SolrType.Double:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToDoubleOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToDouble(attribute.Name) }).ToList()[0].Value;

                case SolrType.Int:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToIntOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToInt(attribute.Name) }).ToList()[0].Value;

                case SolrType.Long:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToLongOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToLong(attribute.Name) }).ToList()[0].Value;

                case SolrType.Short:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToShortOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToShort(attribute.Name) }).ToList()[0].Value;

                case SolrType.Float:
                    if (attribute.IsNullable)
                        return element.Select(e => new { Value = e.ToFloatOrNull(attribute.Name) }).ToList()[0].Value;
                    else
                        return element.Select(e => new { Value = e.ToFloat(attribute.Name) }).ToList()[0].Value;

                case SolrType.String:
                    return element.Select(e => new { Value = e.ToString(attribute.Name) }).ToList()[0].Value;
            }

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
            foreach (object attribute in property.GetCustomAttributes(typeof(T), false))
                if (attribute is T)
                    return (T)attribute;
            return Activator.CreateInstance<T>();
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

    }
}
