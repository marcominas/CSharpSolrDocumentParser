using System;

namespace mmcSolutions.SolrParser.Runner
{
    /// <summary>
    /// A Solr test runner.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Tests.BasicTestResultParser();
            Tests.TestResultParserUsingFileParameter();
            Tests.TestEmployeeParserUsingFileParameter();
            Tests.TestSupervisorParserUsingFileParameter();
            Tests.TestSupervisorParserUsingFileParameterAndSupervisorCriteria();
            Tests.TestManagerParserUsingFileParameter();
            Console.ReadLine();
        }
    }
}
