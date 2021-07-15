using System;
using System.ComponentModel.DataAnnotations;

namespace XCV.Data
{
    public class Role : IComparable
    {
        [Required(ErrorMessage = "Rollen benötigen einen Namen."),
        MaxLength(50, ErrorMessage = "Der Name der Rolle darf 50 Zeichen nicht überschreiten.")]
        public string Name { get; init; }
        public int RCL { get; init; }
        public float Wage { get; set; } //Per hour, i.e. x8 Per day
        public override string ToString() => Name;
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Role other)
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);    //Db-Key is (Name, RCL) cause it would be quite hard to split the table
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Role other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("");
        }
    }
}
