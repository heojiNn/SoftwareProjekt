using System;



namespace Team14.Data
{
    public class Skill : IComparable
    {
        public string Name { get; init; } = "";
        public string Category { get; init; } = "";


        public string Level { get; set; } = "";
        public string[] PossibleLevels { get; set; }
        public SkillGroup Type { get; init; } = SkillGroup.Hardskill;


        public bool HasDouble { get; set; }







        public override string ToString() => $"{Category}\\{Name}-{Level}";

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Skill other)
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is Skill other)
                return this.Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Kann nur mit Skills Vergleichen");
        }

    }




    public enum SkillGroup
    {
        Hardskill,
        Softskill,
    }
}
