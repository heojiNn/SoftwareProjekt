using System;
using System.Collections.Generic;
using System.Linq;

namespace XCV.Data
{
    public class EmployeeConfig
    {
        #nullable enable
        public string? PersNr { get; set; }
        // Personal Data
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime? Experience { get; set; }
        public DateTime? EmployedSince { get; set; }

        // Qualifications
        public ISet<Field>? selectedFields { get; set; }
        public ISet<Skill>? selectedSoftSkills { get; set; }
        public ISet<Skill>? selectedHardSkills { get; set; }
        public ISet<(int project, string activity)>? selectedProjects { get; set; }
        #nullable disable

        public int[] order { get; set; } = new int[5];
    }       
}
