using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mmcSolutions.SolrParser.Sample.DTO.Complex
{
    public class ContactInfo
    {
        [SolrAttribute(Name = "email", Type = SolrType.String, IsNullable = false)]
        public string Email { get; set; }

        [SolrAttribute(Name = "telephone", Type = SolrType.Complex, IsNullable = true)]
        public Telephone Phone { get; set; }

        [SolrAttribute(Name = "cellphone", Type = SolrType.Complex, IsNullable = true)]
        public Telephone CellPhone { get; set; }
    }
}
