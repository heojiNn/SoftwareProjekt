using System;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Field : IComparable
    {
        [Required(ErrorMessage = "Brachen benötigen einen Namen."),
        MaxLength(50, ErrorMessage = "Der Name der Brachen darf 50 Zeichen nicht überschreiten.")]
        public string Name { get; init; } = "";






        public override string ToString() => Name;



        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Field other)
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);    //is Db-Key
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Field other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("");
        }
    }
}
