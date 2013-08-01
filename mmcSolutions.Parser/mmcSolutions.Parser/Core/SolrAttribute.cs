using System;

namespace mmcSolutions.SolrParser
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SolrAttribute : System.Attribute
    {
        /// <summary>
        /// Attribute class to be used with text file parser.
        /// </summary>
        public SolrAttribute() : base() { }

        /// <summary>
        /// The name of field.
        /// </summary>
        public String Name { get; set; }
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
