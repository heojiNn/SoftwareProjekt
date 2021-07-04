using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Project
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Der Titel darf nicht leer sein."),
        MaxLength(20, ErrorMessage = "Der Titel darf nicht länger als 20 Zeichen sein.")]
        public string Title { get; set; } = "";

        [MaxLength(1000, ErrorMessage = "Der Beschreibung darf nicht länger als 1000 Zeichen sein.")]
        public string Description { get; set; } = "";
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;

        public List<string> Purpose { get; set; } = new List<string>();
        public List<Field> Fields { get; set; }
        //List<Field> Fields muss statt string Field in ProjectService.cs genutzt werden!!!!!!!!!!!!!!!
        public string Field { get; set; }


        public Dictionary<string, List<string>> ActivitiesWithEmployees { get; set; } = new();
        // Activities with Reqirements
        public Dictionary<string, IEnumerable<Skill>> Activities { get; set; } = new Dictionary<string, IEnumerable<Skill>>();

        // Members  with Activities
        public Dictionary<Employee, IEnumerable<string>> HasDone { get; set; } = new Dictionary<Employee, IEnumerable<string>>();

    }
}
