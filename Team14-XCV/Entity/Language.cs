using System;


namespace Team14.Data
{
    public class Language : IComparable
    {
        public string Name { get; init; } = "";


        public string Level { get; set; } = "";
        public string[] PossibleLevels { get; set; }






        public override string ToString() => $"{Name}-Niveau-{Level}";

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Language other)
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Language other)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Kann nur mit Sprachen Vergleichen");
        }

    }
}

