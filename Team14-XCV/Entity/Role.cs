using System;



namespace Team14.Data
{
    public class Role : IComparable
    {
        //private TimeSpan _expirience = new();

        public string Name { get; init; } = "";


        public float Expirience
        {
            get;
            set;
        }
        


        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Role other)
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Role other)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Kann nur mit Rollen Vergleichen");
        }

    }

}
