using System;


namespace XCV.Data
{
    public class Skill : SkillCategory, IComparable
    {
        public new string Name { get; init; } = "";

        public string Level { get; set; } = "";
        public string LevelNr { get; set; }
        public SkillGroup Type
        {
            get => Category.Name == "SoftSkills" ? SkillGroup.Softskill : SkillGroup.Hardskill;
        }


        public bool HasDouble { get; set; }
        public SkillCategory Category { get; set; }




        public override string ToString()
        {
            var beginnig = HasDouble ? $"({Category.Name})-" : "";
            return $"{beginnig}{Name}";
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Skill other)
                return Category.Name == other.Category.Name && Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Category.Name, Name);
        public new int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is Skill other)
            {
                var res = Category.Name.CompareTo(other.Category.Name);
                if (res == 0)
                    res = Name.CompareTo(other.Name);
                return res;
            }
            else
                throw new ArgumentException("Kann nur mit Skills vergleichen");
        }
    }



    public enum SkillGroup
    {
        Hardskill,
        Softskill,
    }
}
