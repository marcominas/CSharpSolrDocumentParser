using System;

namespace mmcSolutions.SolrParser
{
    public interface ISolrAttribute
    {
        bool IsNullable { get; set; }
        SolrType Type { get; set; }
    }
}
