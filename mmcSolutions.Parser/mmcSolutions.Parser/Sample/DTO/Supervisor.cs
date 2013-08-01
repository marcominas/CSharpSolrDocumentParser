using System;
using System.Collections.Generic;

namespace mmcSolutions.SolrParser.Sample.DTO
{
    /// <summary>
    /// A Supervisor inherited class from Employee with some specifics properties according to this position.
    /// </summary>
    /// <remarks>This class is for test purpose only.</remarks>
    public class Supervisor : Employee
    {
        /// <summary>
        /// Minimum communications skills
        /// </summary>
        public override Levels MinCommunicationsSkills { get { return Levels.Medium; } }
        /// <summary>
        /// Minimum self-control Skills
        /// </summary>
        public override Levels MinSelfControlSkills { get { return Levels.Above; } }

        /// <summary>
        /// List of subordinates employees.
        /// </summary>
        public List<Employee> Subordinates { get; set; }

    }
}
