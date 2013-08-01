using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mmcSolutions.SolrParser
{
    public class Document : IDocument
    {
        public Document()
        {
            var rnd = new Random();
            DocumentID = rnd.Next();
            Version = rnd.Next();
        }

        public long DocumentID { get; set; }
        public long Version { get; set; }
    }
}
