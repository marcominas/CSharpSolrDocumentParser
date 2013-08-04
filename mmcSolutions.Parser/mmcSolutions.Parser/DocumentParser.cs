using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Collections.Generic;
using mmcSolutions.SolrParser;
using mmcSolutions.SolrParser.Sample.DTO;

namespace mmcSolutions.SolrParser
{
    /// <summary>
    /// Interpreta e extrai os elementos do xml retornado pelo Solr.
    /// </summary>
    public sealed class DocumentParser
    {
        /// <summary>
        /// Parse a Employee from a XML Solr result.
        /// </summary>
        /// <param name="xml">A XML Solr result</param>
        /// <returns>A Employee class with its properties filled acording XML data.</returns>
        internal static Employee ParseEmployee(string xml)
        {
            return ResultParser.Parse<Employee>(xml);
        }
        /// <summary>
        /// Parse a Supervisor from a XML Solr result.
        /// </summary>
        /// <param name="xml">A XML Solr result</param>
        /// <returns>A Supervisor class with its properties filled acording XML data.</returns>
        /// <remarks>This method intend to show how to parse a inherited class from Employee with diferent property.</remarks>
        internal static Supervisor ParseSupervisor(string xml)
        {
            var result = ResultParser.Parse<Supervisor>(xml);
            result.Subordinates = GetEmployees<Employee>(xml, "subordinates");
            return result;
        }
        /// <summary>
        /// Parse a Manager from a XML Solr result.
        /// </summary>
        /// <param name="xml">A XML Solr result</param>
        /// <returns>A Manager class with its properties filled acording XML data.</returns>
        /// <remarks>This method intend to show how to parse a inherited class from Employee with diferent properties.</remarks>
        internal static Manager ParseManager(string xml)
        {
            var result = ResultParser.Parse<Manager>(xml);
            result.Subordinates = GetEmployees<Employee>(xml, "subordinates");
            result.Supervisors = GetEmployees<Supervisor>(xml, "supervisors");
            return result;
        }
        /// <summary>
        /// Parse a Director from a XML Solr result.
        /// </summary>
        /// <param name="xml">A XML Solr result</param>
        /// <returns>A Director class with its properties filled acording XML data.</returns>
        /// <remarks>This method intend to show how to parse a inherited class from Employee with diferent properties.</remarks>
        internal static Director ParseDirector(string xml)
        {
            var result = ResultParser.Parse<Director>(xml);
            result.Subordinates = GetEmployees<Employee>(xml, "subordinates");
            result.Supervisors = GetEmployees<Supervisor>(xml, "supervisors");
            result.Managers = GetEmployees<Manager>(xml, "managers");
            return result;
        }
        /// <summary>
        /// Parse a President from a XML Solr result.
        /// </summary>
        /// <param name="xml">A XML Solr result</param>
        /// <returns>A President class with its properties filled acording XML data.</returns>
        /// <remarks>This method intend to show how to parse a inherited class from Employee with diferent properties.</remarks>
        internal static President ParsePresident(string xml)
        {
            var result = ResultParser.Parse<President>(xml);
            result.Subordinates = GetEmployees<Employee>(xml, "subordinates");
            result.Supervisors = GetEmployees<Supervisor>(xml, "supervisors");
            result.Managers = GetEmployees<Manager>(xml, "managers");
            result.Directors = GetEmployees<Director>(xml, "directors");
            return result;
        }

        #region private methods and other objects.
        
        /// <summary>
        /// Position of each field in a employee string.
        /// </summary>
        private enum EmployeeStringFields
        {
            ID = 0,
            Name = 1,
            Email = 2,
            Role = 3,
            HireDate = 4,
            Address = 5,
            Status = 6
        }

        /// <summary>
        /// Get a T typed list based on Employee properties from a Solr result XML query.
        /// </summary>
        /// <typeparam name="T">Type to be returned.</typeparam>
        /// <param name="xml">A XML Sorl result query.</param>
        /// <param name="arrayName">Name of a array with Employee typed returned in a string array with its fields separated by "|" character.</param>
        /// <returns>List of a T typed objects from a array of employeeString pattern.</returns>
        private static List<T> GetEmployees<T>(string xml, string arrayName)
        {
            var result = new List<T>();
            var list = ResultParser.GetArray<string>(xml, arrayName);
            if (list != null && list.Count > 0)
            {
                foreach (string item in list)
                {
                    var f = ParseEmployeeFromString<T>(item);
                    if (f != null) result.Add(f);
                }
            }
            return result;
        }
        /// <summary>
        /// Get data of a Employee within a string.
        /// </summary>
        /// <typeparam name="T">Type to be returned.</typeparam>
        /// <param name="employeeString">A string with Employee data separated by "|" character.</param>
        /// <returns>A T class with its properties filled acoording data in employee string.</returns>
        private static T ParseEmployeeFromString<T>(string employeeString)
        {
            var fields = employeeString.Split('|');
            return ResultParser.GetInstanceWithPropertiesFilled<T>(new
            {
                ID = GetValue<string>(fields, (short)EmployeeStringFields.ID),
                Name = GetValue<string>(fields, (short)EmployeeStringFields.Name),
                Email = GetValue<string>(fields, (short)EmployeeStringFields.Email),
                HireDate = GetValue<DateTime>(fields, (short)EmployeeStringFields.HireDate),
                Address = GetValue<string>(fields, (short)EmployeeStringFields.Address),
                Status = GetValue<short>(fields, (short)EmployeeStringFields.Status),
            });
        }

        /// <summary>
        /// Get a specific T type value in a string array.
        /// </summary>
        /// <typeparam name="T">Type to be returned.</typeparam>
        /// <param name="fields">A array with values.</param>
        /// <param name="index">Index of value in array.</param>
        /// <returns>A T value content of item in array.</returns>
        private static T GetValue<T>(string[] fields, int index)
        {
            T result = default(T);
            var type = typeof(T);

            try
            {
                if (type.Equals(typeof(string)))
                    return (T)Convert.ChangeType(fields[index], type);
                if (type.Equals(typeof(DateTime)))
                    return (T)Convert.ChangeType(DateTime.Parse(fields[index]), type);
                if (type.Equals(typeof(short)))
                    return (T)Convert.ChangeType(short.Parse(fields[index]), type);
            }
            catch { }
            return result;
        }
        /// <summary>
        /// Identify the employee type by role in XML result.
        /// </summary>
        /// <param name="xml">A xml Solr query result.</param>
        /// <returns>The role in found XML otherwise null.</returns>
        internal static string GetEmployeeRole(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var xpdoc = new XPathDocument(reader);
                var xpnav = xpdoc.CreateNavigator();
                var node = xpnav.SelectSingleNode("//*[@name='role']");
                return node == null ? null : node.Value;
            }
        }
        
        #endregion

    }
}
