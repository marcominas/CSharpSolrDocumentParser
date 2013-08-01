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
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>A list populated with the values ​​found in the array specified XML.</returns>
        public static List<string> ToArray(this XElement elemento, string atributo)
        {
            try 
            { 
                var elementos = elemento.Elements("arr").First(s => s.Attribute("name").Value.Equals(atributo));
                var result = new List<string>();
                if (elementos != null & !string.IsNullOrEmpty(elementos.Value))
                    foreach (string foto in elementos.Value.Split(']'))
                        if (!string.IsNullOrEmpty(foto)) result.Add(foto);
                return result;
                //return GetValue(e, "array", atributo); 
            }
            catch { return null; }
        }

        /// <summary>
        /// Gets a string contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or a string empty if any error.</returns>
        public static string ToString(this XElement elemento, string atributo)
        {
            try { return GetValue(elemento, "str", atributo); }
            catch { return string.Empty; }
        }
        /// <summary>
        /// Gets a string contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or a string empty if any error.</returns>
        public static string ToText(this XElement elemento, string atributo)
        {
            try { return GetValue(elemento, "text", atributo); }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Gets a DateTime contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or default value of a DateTime if any error.</returns>
        public static DateTime ToDate(this XElement elemento, string atributo)
        {
            try { return DateTime.Parse(GetValue(elemento, "date", atributo), CultureInfo.InvariantCulture); }
            catch { return default(DateTime); }
        }
        /// <summary>
        /// Gets a DateTime contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static DateTime? ToDateOrNull(this XElement elemento, string atributo)
        {
            try { return DateTime.Parse(GetValue(elemento, "date", atributo), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a boolean contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or boolean default value if any error.</returns>
        public static bool ToBoolean(this XElement elemento, string atributo)
        {
            try { return bool.Parse(GetValue(elemento, "bool", atributo)); }
            catch { return default(bool); }
        }
        /// <summary>
        /// Gets a boolean contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static bool? ToBooleanOrNull(this XElement elemento, string atributo)
        {
            try { return bool.Parse(GetValue(elemento, "bool", atributo)); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a short contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or short default value if any error.</returns>
        public static short ToShort(this XElement elemento, string atributo)
        {
            try { return short.Parse(GetValue(elemento, "short", atributo), CultureInfo.InvariantCulture); }
            catch { return default(short); }
        }
        /// <summary>
        /// Gets a short contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static short? ToShortOrNull(this XElement elemento, string atributo)
        {
            try { return short.Parse(GetValue(elemento, "short", atributo), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a int contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or int default value if any error.</returns>
        public static int ToInt(this XElement elemento, string atributo)
        {
            try { return int.Parse(GetValue(elemento, "int", atributo), CultureInfo.InvariantCulture); }
            catch { return default(short); }
        }
        /// <summary>
        /// Gets a int contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static int? ToIntOrNull(this XElement elemento, string atributo)
        {
            try { return (Nullable<int>)int.Parse(GetValue(elemento, "int", atributo), CultureInfo.InvariantCulture); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a long contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or long default value if any error.</returns>
        public static long ToLong(this XElement elemento, string atributo)
        {
            try { return short.Parse(GetValue(elemento, "long", atributo), CultureInfo.InvariantCulture); }
            catch { return default(long); }
        }
        /// <summary>
        /// Gets a long contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static long? ToLongOrNull(this XElement elemento, string atributo)
        {
            try { return long.Parse(GetValue(elemento, "long", atributo), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a decimal contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or decimal default value if any error.</returns>
        public static decimal ToDecimal(this XElement elemento, string atributo)
        {
            try { return decimal.Parse(GetValue(elemento, "decimal", atributo), CultureInfo.InvariantCulture); }
            catch { return default(decimal); }
        }
        /// <summary>
        /// Gets a decimal contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static decimal? ToDecimalOrNull(this XElement elemento, string atributo)
        {
            try { return decimal.Parse(GetValue(elemento, "decimal", atributo), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a double contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or double default value if any error.</returns>
        public static double ToDouble(this XElement elemento, string atributo)
        {
            try { return double.Parse(GetValue(elemento, "double", atributo), CultureInfo.InvariantCulture); }
            catch { return default(double); }
        }
        /// <summary>
        /// Gets a double contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static double? ToDoubleOrNull(this XElement elemento, string atributo)
        {
            try { return double.Parse(GetValue(elemento, "double", atributo), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets a float contained in an Linq xml element.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or float default value if any error.</returns>
        public static float ToFloat(this XElement elemento, string atributo)
        {
            try { return float.Parse(GetValue(elemento, "float", atributo), CultureInfo.InvariantCulture); }
            catch { return default(float); }
        }
        /// <summary>
        /// Gets a float contained in an Linq xml element and may return a null value.
        /// </summary>
        /// <param name="elemento">The Linq XML element that contains the data.</param>
        /// <param name="atributo">Name of the attribute that you want to get the value.</param>
        /// <returns>The value if the attribute is found or null if any error.</returns>
        public static float? ToFloatOrNull(this XElement elemento, string atributo)
        {
            try { return float.Parse(GetValue(elemento, "float", atributo), CultureInfo.InvariantCulture); }
            catch { return null; }
        }

        /// <summary>
        /// Gets the value of an element in an XML coming from Solr's name and attribute specified.
        /// </summary>
        /// <param name="elemento">Element(s) that contains the XML data to be returned.</param>
        /// <param name="elementName">Node(s) name to be extracted from informed elements.</param>
        /// <param name="atributo">Name of the attribute that you want to get value(s).</param>
        /// <returns></returns>
        private static string GetValue(XElement elemento, string elementName, string atributo)
        {
            return elemento.Elements(elementName).First(s => s.Attribute("name").Value.Equals(atributo)).Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string RemoveNodeName(this XNode node, string name)
        {
            return node.ToString()
                       .Replace(string.Format("<{0}>", name), "")
                       .Replace(string.Format("</{0}>", name), "");
        }

    }
}
