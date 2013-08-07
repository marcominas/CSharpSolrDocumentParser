using System;

namespace mmcSolutions.SolrParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SolrComplexAttribute : Attribute, ISolrAttribute
    {
        /// <summary>
        /// Prefix used to mount field name.
        /// </summary>
        public String Prefix { get; set; }
        /// <summary>
        /// String used to concatenate
        /// </summary>
        public string Separator { get; set; }
        /// <summary>
        /// Specify Solr data type.
        /// </summary>
        public SolrType Type { get; set; }
        /// <summary>
        /// Specify if can be null
        /// </summary>
        public bool IsNullable { get; set; }

    }
}
