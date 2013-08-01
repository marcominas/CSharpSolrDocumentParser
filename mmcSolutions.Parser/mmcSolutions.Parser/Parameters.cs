using System;
using System.Collections.Generic;

namespace mmcSolutions.SolrParser
{
    public class Parameters
    {
        public Parameters()
        {
            Criteria = new Dictionary<string, string>();
        }
        public string SolrURL { get; set; }
        public string SolrResultFile { get; set; }
        public Dictionary<string, string> Criteria { get; set; }
    }
}
