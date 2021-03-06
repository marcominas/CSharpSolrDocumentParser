﻿using System;

namespace mmcSolutions.SolrParser
{
    /// <summary>
    /// Attribute class to be used with text file parser.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SolrAttribute : Attribute, ISolrAttribute
    {
        /// <summary>
        /// Field name.
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
