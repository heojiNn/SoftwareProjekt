using System;
using System.ComponentModel.DataAnnotations;

namespace Team14.Data
{
    public class Skill
    {
        public Skill()
        {
        }

        [Key]
        public int iD { get; set; }
        public string Name { get; set; }
        public Category Kategorisierung { get; set; }

        public enum Category
        {
            Hardskill,
            Softskill,
        }

    }    
}
