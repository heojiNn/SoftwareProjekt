using System;
using System.Collections.Generic;
using System.Linq;

namespace Team14.Data
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;
        public string Purpose { get; set; } = "";

        public IEnumerable<Field> Fields { get; set; } = new List<Field>();




        // Activities with Reqirements
        public Dictionary<string, IEnumerable<Skill>> Activities { get; set; } = new Dictionary<string, IEnumerable<Skill>>();

        // Members  with Activities
        public Dictionary<Employee, IEnumerable<string>> HasDone { get; private set; } = new Dictionary<Employee, IEnumerable<string>>();


    }
}
