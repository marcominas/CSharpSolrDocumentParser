using System;
using System.Collections.Generic;

namespace mmcSolutions.SolrParser.Sample.DTO
{
    public class Manager : Employee
    {
        /// <summary>
        /// Minimum communications skills
        /// </summary>
        public override Levels MinCommunicationsSkills { get { return Levels.Above; } }
        /// <summary>
        /// Minimum self-control Skills
        /// </summary>
        public override Levels MinSelfControlSkills { get { return Levels.High; } }

        public List<Supervisor> Supervisors { get; set; }
        public List<Employee> Subordinates { get; set; }
    }
}
