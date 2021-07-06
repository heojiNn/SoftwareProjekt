using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Offer
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]{2,30}$",
            ErrorMessage = "zu lang oder falsch")]
        public string Title { get; set; } = "";
        [MaxLength(140, ErrorMessage = "Beschreibung nicht über 140 Zeichen")]
        public string Description { get; set; } = "";
        public IList<Skill> Requirements { get; set; } = new List<Skill>();
        public IList<Field> Fields { get; set; } = new List<Field>();
        public IList<Employee> participants { get; set; } = new List<Employee>();
        public ISet<(Role, Employee)> participantRoles { get; set; } = new SortedSet<(Role, Employee)>();
        public IList<(Employee, int)> participantRCLs { get; set; } = new List<(Employee, int)>();
    }
}
