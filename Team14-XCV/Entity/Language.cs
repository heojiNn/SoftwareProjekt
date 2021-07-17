using System;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Language : IComparable
    {
        [Required(ErrorMessage = "Sprachen benötigen einen Namen."),
        MaxLength(50, ErrorMessage = "Der Name der Sprache darf 50 Zeichen nicht überschreiten.")]
        public string Name { get; set; } = "";

        public string Level { get; set; } = "";



        public override string ToString() => $"{Name}-auf-Niveau-{Level}";



        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Language other)
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);    //is Db-Key
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Language other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("");
        }
    }
}
