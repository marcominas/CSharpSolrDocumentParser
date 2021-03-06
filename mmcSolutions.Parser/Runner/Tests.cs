﻿using System;
using System.IO;
using System.Text;
using System.Xml;

namespace mmcSolutions.SolrParser.Runner
{
    /// <summary>
    /// Parse results Solr response to Data Transfer Object (DTO) sample queries code.
    /// </summary>
    public class Tests
    {
        private const string DATA_PATH = @"..\..\..\mmcSolutions.Parser\Sample\Data\";
        private static string GetFullPathSampleFile(string filename)
        {
            var dataPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, DATA_PATH));

            if (!Directory.Exists(dataPath))
                throw new DirectoryNotFoundException(string.Format("Path not found: {0}", dataPath));

            var filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, DATA_PATH, filename));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(string.Format("File not found: {0}", dataPath));

            return filePath;
        }

        /// <summary>
        /// Parse a Employee DTO from a XML result query sample.
        /// </summary>
        public static void BasicTestResultParser()
        {
            // XML Solr query result sample that contains one employee valid data.
            var file = GetFullPathSampleFile(@"Employee.xml");
            var xml = File.ReadAllText(file, Encoding.Unicode);

            Console.WriteLine();
            Console.WriteLine("Basic test result parser.");

            foreach (XmlNode doc in ResultParser.GetSolrDocumentsFromXML(xml))
            {
                var obj = ResultParser.Parse<Sample.DTO.Employee>(doc.OuterXml);
                Console.WriteLine(string.Format("ID {0}; Name {1}; e-mail {2}", obj.ID, obj.Name, obj.Email));
            }
        }

        /// <summary>
        /// Parse a list of Employee and Employee inherited class DTOs from a XML result query sample.
        /// </summary>
        public static void TestResultParserUsingFileParameter()
        {
            var param = ParameterWithFile();
            var result = SearchEngine.Search(param);

            Console.WriteLine();
            Console.WriteLine("Test - Documents parser using search engine.");

            foreach (var doc in result)
                Console.WriteLine(string.Format("ID {0}; Version {1}", doc.DocumentID, doc.Version));
        }

        /// <summary>
        /// Parse a list of Employee class DTOs from a XML result query sample.
        /// </summary>
        public static void TestEmployeeParserUsingFileParameter()
        {
            var param = ParameterWithFile();
            var result = SearchEngine.Search<Sample.DTO.Employee>(param);

            Console.WriteLine();
            Console.WriteLine("Test - Employee parser using parameter without role.");

            foreach (Sample.DTO.Employee doc in result)
                Console.WriteLine(string.Format("{0}; {1}; {2}", doc.ID, doc.Name, doc.Email));
        }
        /// <summary>
        /// Parse a list of Supervisor Employee inherited class DTOs from a XML result query sample.
        /// </summary>
        /// <remarks>It's not necessary pass role parameter because the Supervisor type is specified and Search<T>() method treats this criterion.</remarks>
        public static void TestSupervisorParserUsingFileParameter()
        {
            var param = ParameterWithFile();
            // The search method will create a supervisor role.
            var result = SearchEngine.Search<Sample.DTO.Supervisor>(param);

            Console.WriteLine();
            Console.WriteLine("Test - Supervisor parser using parameter without role.");

            foreach (Sample.DTO.Supervisor doc in result)
                Console.WriteLine(string.Format("{4}\r\n  ID...........: {0};\r\n  Name.........: {1};\r\n  E-mail.......: {2};\r\n  Subordinates.: {3}\r\n{5}",
                                    doc.ID, doc.Name, doc.Email, doc.Subordinates.Count, "{", "}"));
        }
        /// <summary>
        /// Parse a list of Supervisor Employee inherited class DTOs from a XML result query sample.
        /// </summary>
        /// <remarks>If is passed role parameter the Search<T>() method treats this criterion to not duplicate it.</remarks>
        public static void TestSupervisorParserUsingFileParameterAndSupervisorCriteria()
        {
            var param = ParameterWithFile();
            param.Criteria.Add("role", "Supervisor");
            // The search method won't duplicate supervisor role.
            var result = SearchEngine.Search<Sample.DTO.Supervisor>(param);

            Console.WriteLine();
            Console.WriteLine("Test - Supervisor parser using parameter without role.");

            foreach (Sample.DTO.Supervisor doc in result)
            {
                var subordinates = string.Empty;
                foreach (Sample.DTO.Employee emp in doc.Subordinates)
                    subordinates += string.Format("      {0} - {1}\r\n", emp.Name, emp.Email);
                Console.WriteLine(string.Format("{4}\r\n  ID...........: {0};\r\n  Name.........: {1};\r\n  E-mail.......: {2};\r\n  Subordinates.: {3}\r\n{6}\r\n{5}",
                                    doc.ID, doc.Name, doc.Email, doc.Subordinates.Count, "{", "}", subordinates));
            }
        }
        /// <summary>
        /// Parse a list of Supervisor Employee inherited class DTOs from a XML result query sample.
        /// </summary>
        public static void TestManagerParserUsingFileParameter()
        {
            var param = ParameterWithFile();
            // The search method will create a manager role.
            var result = SearchEngine.Search<Sample.DTO.Manager>(param);

            Console.WriteLine();
            Console.WriteLine("Test - Supervisor parser using parameter without role.");

            foreach (Sample.DTO.Manager doc in result)
            {
                var subordinates = string.Empty;
                var supervisors = string.Empty;
                foreach (Sample.DTO.Employee emp in doc.Supervisors)
                    supervisors += string.Format("      {0} - {1}\r\n", emp.Name, emp.Email);
                foreach (Sample.DTO.Employee emp in doc.Subordinates)
                    subordinates += string.Format("      {0} - {1}\r\n", emp.Name, emp.Email);
                Console.WriteLine(string.Format("{4}\r\n  ID...........: {0};\r\n  Name.........: {1};\r\n  E-mail.......: {2};\r\n  Supervisors.: {3}\r\n{6}  Subordinates.: {7}\r\n{8}\r\n{5}",
                                    doc.ID, doc.Name, doc.Email, doc.Supervisors.Count, "{", "}", supervisors, doc.Subordinates.Count, subordinates));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Parameters ParameterWithFile()
        {
            var file = GetFullPathSampleFile(@"Employees.xml");
            return new Parameters { SolrResultFile = file };
        }

        /// <summary>
        /// The DTO Person contains a <see cref="ContactInfo">ContactInfo</see> property that contains a e-mail property and two <see cref="Telephone">Telephone</see> properties.
        /// This sample is about parse a Person class properly with its class properties too.
        /// </summary>
        public static void ComplexTestResultParser()
        {
            var file = GetFullPathSampleFile(@"Complex.xml");
            var param = new Parameters { SolrResultFile = file };
            // The search method will create a supervisor role.
            var result = SearchEngine.Search<Sample.DTO.Complex.Person>(param);

            Console.WriteLine();
            Console.WriteLine("Test - Parse a complex DTO that contains others DTO in its properties.");

            foreach (Sample.DTO.Complex.Person doc in result)
                Console.WriteLine(string.Format("\r\n  ID...........: {0};\r\n  Name.........: {1};\r\n  E-mail.......: {2};\r\n  Telephone....: ({3}) {4};\r\n  Cellphone....: ({5}) {6}",
                                    doc.ID, doc.Name, doc.ContactInfo.Email, doc.ContactInfo.Phone.Area, doc.ContactInfo.Phone.Number, doc.ContactInfo.CellPhone.Area, doc.ContactInfo.CellPhone.Number));
        }

    }
}
