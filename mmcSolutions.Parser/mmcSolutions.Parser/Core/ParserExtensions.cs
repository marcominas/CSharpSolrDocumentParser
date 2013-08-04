using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace mmcSolutions.SolrParser
{
    public static class ParserExtensions
    {
        /// <summary>
        /// Gets a list of string contained in an Linq xml element defined as array.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>A list populated with the values ​​found in the array specified XML.</returns>
        public static List<T> ToArray<T>(this XElement element, string attribute)
        {
            var result = new List<T>();
            var elements = element.Elements("arr").First(s => s.Attribute("name").Value.Equals(attribute));
            if (elements != null && !elements.IsEmpty)
            {
                foreach (var descendant in elements.Descendants())
                    if (!string.IsNullOrEmpty(descendant.Value))
                        try { result.Add((T)Convert.ChangeType(descendant.Value, typeof(T))); }
                        catch { }
            }
            return result.Count > 0 ? result : null;
        }

        /// <summary>
        /// Gets a string contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or a string empty if any error.</returns>
        public static string ToString(this XElement element, string attribute)
        {
            try { return GetValue(element, "str", attribute); }
            catch { return string.Empty; }
        }
        /// <summary>
        /// Gets a string contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or a string empty if any error.</returns>
        public static string ToText(this XElement element, string attribute)
        {
            try { return GetValue(element, "text", attribute); }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Gets a DateTime contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or default value of a DateTime if any error.</returns>
        public static DateTime ToDate(this XElement element, string attribute)
        {
            try 
            {
                var value = GetValue(element, "date", attribute).Replace("Z", "").Replace("T", " ");
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            catch { return default(DateTime); }
        }
        /// <summary>
        /// Gets a DateTime contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static DateTime? ToDateOrNull(this XElement element, string attribute)
        {
            try 
            {
                var value = GetValue(element, "date", attribute).Replace("Z", "").Replace("T", " ");
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            catch { return null; }
        }

        /// <summary>
        /// Gets a boolean contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or boolean default value if any error.</returns>
        public static bool ToBoolean(this XElement element, string attribute)
        {
            try { return bool.Parse(GetValue(element, "bool", attribute)); }
            catch { return default(bool); }
        }
        /// <summary>
        /// Gets a boolean contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static bool? ToBooleanOrNull(this XElement element, string attribute)
        {
            try { return bool.Parse(GetValue(element, "bool", attribute)); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a short contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or short default value if any error.</returns>
        public static short ToShort(this XElement element, string attribute)
        {
            try { return short.Parse(GetValue(element, "short", attribute), CultureInfo.InvariantCulture); }
            catch { return default(short); }
        }
        /// <summary>
        /// Gets a short contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static short? ToShortOrNull(this XElement element, string attribute)
        {
            try { return short.Parse(GetValue(element, "short", attribute), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a int contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or int default value if any error.</returns>
        public static int ToInt(this XElement element, string attribute)
        {
            try { return int.Parse(GetValue(element, "int", attribute), CultureInfo.InvariantCulture); }
            catch { return default(int); }
        }
        /// <summary>
        /// Gets a int contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static int? ToIntOrNull(this XElement element, string attribute)
        {
            try { return (Nullable<int>)int.Parse(GetValue(element, "int", attribute), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a long contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or long default value if any error.</returns>
        public static long ToLong(this XElement element, string attribute)
        {
            try { return long.Parse(GetValue(element, "long", attribute), CultureInfo.InvariantCulture); }
            catch { return default(long); }
        }
        /// <summary>
        /// Gets a long contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static long? ToLongOrNull(this XElement element, string attribute)
        {
            try { return long.Parse(GetValue(element, "long", attribute), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a decimal contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or decimal default value if any error.</returns>
        public static decimal ToDecimal(this XElement element, string attribute)
        {
            try { return decimal.Parse(GetValue(element, "decimal", attribute), CultureInfo.InvariantCulture); }
            catch { return default(decimal); }
        }
        /// <summary>
        /// Gets a decimal contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static decimal? ToDecimalOrNull(this XElement element, string attribute)
        {
            try { return decimal.Parse(GetValue(element, "decimal", attribute), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a double contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or double default value if any error.</returns>
        public static double ToDouble(this XElement element, string attribute)
        {
            try { return double.Parse(GetValue(element, "double", attribute), CultureInfo.InvariantCulture); }
            catch { return default(double); }
        }
        /// <summary>
        /// Gets a double contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static double? ToDoubleOrNull(this XElement element, string attribute)
        {
            try { return double.Parse(GetValue(element, "double", attribute), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a float contained in an Linq xml element.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or float default value if any error.</returns>
        public static float ToFloat(this XElement element, string attribute)
        {
            try { return float.Parse(GetValue(element, "float", attribute), CultureInfo.InvariantCulture); }
            catch { return default(float); }
        }
        /// <summary>
        /// Gets a float contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="element">The Linq XML element that contains the data.</param>
        /// <param name="attribute">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static float? ToFloatOrNull(this XElement element, string attribute)
        {
            try { return float.Parse(GetValue(element, "float", attribute), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        public static object ToComplex(this XElement element, string attribute, string xml, System.Reflection.PropertyInfo pi)
        {
            try { return ResultParser.Parse(xml, pi, attribute); }
            catch { return default(object); }
        }


        /// <summary>
        /// Gets the value of an element in an XML coming from Solr's name and attribute specified.
        /// </summary>
        /// <param name="element">Element(s) that contains the XML data to be returned.</param>
        /// <param name="elementName">Node(s) name to be extracted from informed elements.</param>
        /// <param name="attribute">Name of the attribute that you want to get value(s).</param>
        /// <returns></returns>
        private static string GetValue(XElement element, string elementName, string attribute)
        {
            return element.Elements(elementName).First(s => s.Attribute("name").Value.Equals(attribute)).Value;
        }
        /// <summary>
        /// Remove XML tags specified in <see cref="name">name</see> parameter.
        /// </summary>
        /// <param name="node">A XML node.</param>
        /// <param name="name">Name of tag to be removed.</param>
        /// <returns></returns>
        public static string RemoveNodeName(this XNode node, string name)
        {
            return node.ToString()
                       .Replace(string.Format("<{0}>", name), "")
                       .Replace(string.Format("</{0}>", name), "");
        }

    }
}
