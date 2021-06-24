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


        public Object DocumentSettings { get; set; }


        // might be null
        public IEnumerable<(Role, Employee)> Members { get; set; } = new List<(Role, Employee)>();


        public IEnumerable<Field> Fields { get; set; } = new List<Field>();
        public IEnumerable<Skill> Requierments { get; set; } = new List<Skill>();

    }
}
