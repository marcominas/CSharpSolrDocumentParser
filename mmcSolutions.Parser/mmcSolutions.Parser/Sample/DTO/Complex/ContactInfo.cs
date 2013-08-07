using System;

namespace mmcSolutions.SolrParser.Sample.DTO.Complex
{
    public class ContactInfo
    {
        [SolrAttribute(Name = "email", Type = SolrType.String, IsNullable = false)]
        public string Email { get; set; }

        [SolrComplexAttribute(Prefix = "telephone", Separator = "_", Type = SolrType.Complex, IsNullable = true)]
        public Telephone Phone { get; set; }

        [SolrComplexAttribute(Prefix = "cellphone", Separator = "_", Type = SolrType.Complex, IsNullable = true)]
        public Telephone CellPhone { get; set; }
    }
}
