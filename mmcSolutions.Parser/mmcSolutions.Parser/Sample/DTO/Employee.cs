using System;

namespace mmcSolutions.SolrParser.Sample.DTO
{
    /// <summary>
    /// A sample Employee entity.
    /// </summary>
    public class Employee: IDocument
    {
        public Employee()
        {
            var rnd = new Random();
            DocumentID = rnd.Next();
            Version = rnd.Next();
        }
        /// <summary>
        /// Employee ID.
        /// </summary>
        [SolrAttribute(Name = "id", Type = SolrType.String, IsNullable = false)]
        public string ID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [SolrAttribute(Name = "name", Type = SolrType.String, IsNullable = false)]
        public string Name { get; set; }
        /// <summary>
        /// E-mail
        /// </summary>
        [SolrAttribute(Name = "email", Type = SolrType.String, IsNullable = false)]
        public string Email { get; set; }
        /// <summary>
        /// Hire date
        /// </summary>
        [SolrAttribute(Name = "hiredate", Type = SolrType.Date, IsNullable = false)]
        public DateTime HireDate { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [SolrAttribute(Name = "address", Type = SolrType.String, IsNullable = false)]
        public string Address { get; set; }
        /// <summary>
        /// Manager
        /// </summary>
        [SolrAttribute(Name = "sallary", Type = SolrType.Float, IsNullable = false)]
        public float Sallary { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [SolrAttribute(Name = "status", Type = SolrType.Short, IsNullable = false)]
        public short Status { get; set; }
        /// <summary>
        /// Demission date.
        /// </summary>
        [SolrAttribute(Name = "demissiondate", Type = SolrType.Date, IsNullable = true)]
        public DateTime? DemissionDate { get; set; }
        /// <summary>
        /// Photo ID.
        /// </summary>
        [SolrAttribute(Name = "photoid", Type = SolrType.Long, IsNullable = true)]
        public long? PhotoId { get; set; }
        /// <summary>
        /// Percentage discount
        /// </summary>
        [SolrAttribute(Name = "discount", Type = SolrType.Float, IsNullable = true)]
        public float? PercentageDiscount { get; set; }
        /// <summary>
        /// Communications skills
        /// </summary>
        [SolrAttribute(Name = "communicationsskills", Type = SolrType.Float, IsNullable = true)]
        public virtual Levels CommunicationsSkills { get; set; }
        /// <summary>
        /// Self-control Skills
        /// </summary>
        [SolrAttribute(Name = "selfcontrolskills", Type = SolrType.Float, IsNullable = true)]
        public virtual Levels SelfControlSkills { get; set; }

        /// <summary>
        /// Minimum communications skills
        /// </summary>
        public virtual Levels MinCommunicationsSkills { get { return Levels.Basic; } }
        /// <summary>
        /// Minimum self-control Skills
        /// </summary>
        public virtual Levels MinSelfControlSkills { get { return Levels.Basic; } }

        /// <summary>
        /// Validate if level skills are compatible with position of employee.
        /// </summary>
        public bool IsLevelSkillsValid 
        {
            get 
            { 
                return 
                        CommunicationsSkills > MinCommunicationsSkills 
                        && 
                        SelfControlSkills > MinSelfControlSkills; 
            }
        }


        public long DocumentID { get; set; }

        public long Version { get; set; }

    }
}
