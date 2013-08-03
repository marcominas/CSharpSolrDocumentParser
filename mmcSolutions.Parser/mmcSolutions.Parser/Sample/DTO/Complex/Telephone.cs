using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mmcSolutions.SolrParser.Sample.DTO.Complex
{
    public class Telephone
    {

        [SolrAttribute(Name = "country", Type = SolrType.String, IsNullable = true)]
        public string Country { get; set; }

        [SolrAttribute(Name = "area", Type = SolrType.String, IsNullable = false)]
        public string Area { get; set; }

        [SolrAttribute(Name = "number", Type = SolrType.String, IsNullable = false)]
        public string Number { get; set; }

    }
}
