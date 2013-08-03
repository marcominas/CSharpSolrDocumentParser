using System;

namespace mmcSolutions.SolrParser.Sample.DTO.Complex
{
    public class Person
    {
        [SolrAttribute(Name = "id", Type = SolrType.Long, IsNullable = false)]
        public long ID { get; set; }

        [SolrAttribute(Name = "name", Type = SolrType.String, IsNullable = false)]
        public string Name { get; set; }

        [SolrAttribute(Name = "birthdate", Type = SolrType.Date, IsNullable = false)]
        public DateTime BirthDate { get; set; }

        [SolrAttribute(Name = "contactinfo", Type = SolrType.Complex, IsNullable = true)]
        public ContactInfo ContactInfo { get; set; }

    }
}
