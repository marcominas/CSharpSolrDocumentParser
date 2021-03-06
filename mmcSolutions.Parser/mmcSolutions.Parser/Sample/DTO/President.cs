﻿using System;
using System.Collections.Generic;

namespace mmcSolutions.SolrParser.Sample.DTO
{
    /// <summary>
    /// A President inherited class from Employee with some specifics properties according to this position.
    /// </summary>
    /// <remarks>This class is for test purpose only.</remarks>
    public class President : Employee
    {
        /// <summary>
        /// Minimum communications skills
        /// </summary>
        public override Levels MinCommunicationsSkills { get { return Levels.Ultra; } }
        /// <summary>
        /// Minimum self-control Skills
        /// </summary>
        public override Levels MinSelfControlSkills { get { return Levels.Ultra; } }

        public List<Director> Directors { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Supervisor> Supervisors { get; set; }
        public List<Employee> Subordinates { get; set; }
    }
}
