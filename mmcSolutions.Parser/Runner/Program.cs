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

            Tests.ComplexTestResultParser();

            Tests.BasicTestResultParser();
            
            Tests.TestResultParserUsingFileParameter();
            
            // tests using DTO
            Tests.TestEmployeeParserUsingFileParameter();
            Tests.TestSupervisorParserUsingFileParameter();
            Tests.TestSupervisorParserUsingFileParameterAndSupervisorCriteria();
            Tests.TestManagerParserUsingFileParameter();
            
            Console.ReadLine();
        }
    }
}
