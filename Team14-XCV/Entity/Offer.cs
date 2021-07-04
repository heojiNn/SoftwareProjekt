using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Offer
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Za-z ]{2,30}$",
            ErrorMessage = "zu lang oder falsch")]
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public ISet<Skill> Requirements { get; set; } = new SortedSet<Skill>();
        public ISet<Field> Fields { get; set; } = new SortedSet<Field>();
        public ISet<Employee> participants { get; set; } = new SortedSet<Employee>();

        public IEnumerable<(Role, Employee)> participantRoles { get; set; } = new List<(Role, Employee)>();
        public IEnumerable<(Employee, int)> participantRCLs { get; set; } = new List<(Employee, int)>();
    }
}
