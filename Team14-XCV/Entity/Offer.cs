using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Offer
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Za-z0-9_\-,.]{1,50}$",
            ErrorMessage = "darf nur Buchstaben Zahlen oder -_,. enthalten")]
        public string Title { get; set; } = "";
        [MaxLength(140, ErrorMessage = "Beschreibung nicht über 140 Zeichen")]
        public string Description { get; set; } = "";
        public DateTime Start { get; set; } 
        public DateTime End { get; set; } 

        public ISet<Skill> Requirements { get; set; } = new SortedSet<Skill>();
        public ISet<Field> Fields { get; set; } = new SortedSet<Field>();
        public IList<Employee> participants { get; set; } = new List<Employee>();
    }
}
