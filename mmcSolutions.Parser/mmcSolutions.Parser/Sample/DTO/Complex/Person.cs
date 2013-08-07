using System;
using System.Collections.Generic;

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

        [SolrComplexAttribute(Prefix = "contactinfo", Separator = "_", Type = SolrType.Complex, IsNullable = true)]
        public ContactInfo ContactInfo { get; set; }

        [SolrAttribute(Name = "friends", Type = SolrType.Array, IsNullable = true)]
        public List<string> Friends { get; set; }

        [SolrAttribute(Name = "married", Type = SolrType.Bool, IsNullable = true)]
        public bool Married { get; set; }

        [SolrAttribute(Name = "numberofchildren", Type = SolrType.Short, IsNullable = true)]
        public short NumberOfChildren { get; set; }

        [SolrAttribute(Name = "weight", Type = SolrType.Decimal, IsNullable = true)]
        public decimal Weight { get; set; }

        [SolrAttribute(Name = "height", Type = SolrType.Float, IsNullable = true)]
        public float Height { get; set; }

    }
}
