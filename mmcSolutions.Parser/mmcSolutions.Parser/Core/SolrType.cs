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
        /// Binary solr type
        /// </summary>
        Binary,
        /// <summary>
        /// Array solr type
        /// </summary>
        Array,

        /// <summary>
        /// DateTime solr type
        /// </summary>
        Date,

        /// <summary>
        /// Byte solr type
        /// </summary>
        Byte,
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
        /// Currency solr type
        /// </summary>
        Currency,

        /// <summary>
        /// Latitide/longitude coordinate solr type
        /// </summary>
        Coordinate,

        /// <summary>
        /// Complex data type
        /// </summary>
        Complex

    }
}
