using System;

namespace mmcSolutions.SolrParser
{
    public interface IDocument
    {
        long DocumentID { get; set; }
        long Version { get; set; }

    }
}
