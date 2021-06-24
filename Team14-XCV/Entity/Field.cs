using System;


namespace XCV.Data
{
    public class Field : IComparable
    {
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
        public override int GetHashCode() => HashCode.Combine(Name);
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Field other)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Kann nur mit Brachen vergleichen");
        }
    }
}
