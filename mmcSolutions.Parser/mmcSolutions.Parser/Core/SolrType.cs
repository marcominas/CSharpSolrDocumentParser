using System;

namespace mmcSolutions.SolrParser
{
    /// <summary>
    /// 
    /// </summary>
    public enum SolrType
    {
        /// <summary>
        /// Not convertible type
        /// </summary>
        NotConvertible,
        /// <summary>
        /// String solr type
        /// </summary>
        String,
        /// <summary>
        /// Array solr type
        /// </summary>
        Array,
        /// <summary>
        /// DateTime solr type
        /// </summary>
        Date,
        /// <summary>
        /// Boolean
        /// </summary>
        Bool,
        /// <summary>
        /// Int solr type
        /// </summary>
        Int,
        /// <summary>
        /// Short solr type
        /// </summary>
        Short,
        /// <summary>
        /// Long solr type
        /// </summary>
        Long,
        /// <summary>
        /// Decimal solr type
        /// </summary>
        Decimal,
        /// <summary>
        /// Double solr type
        /// </summary>
        Double,
        /// <summary>
        /// Float
        /// </summary>
        Float,
        /// <summary>
        /// Complex data type
        /// </summary>
        Complex
    }
}
