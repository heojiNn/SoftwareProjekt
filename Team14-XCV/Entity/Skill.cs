using System;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Skill : SkilTreeNode, IComparable
    {
        [Required(ErrorMessage = "Skills benötigen einen Namen."),
        MaxLength(50, ErrorMessage = "Der Name des Skill darf {1} Zeichen nicht überschreiten.")]
        public override string Name { get; set; } = "";

        public string Level { get; set; } = "";                         // SQL-Key would be the associated nummber,  cause names can be changed
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
        public override int GetHashCode() => HashCode.Combine(Category.Name, Name);    //is Db-Key
        public int CompareTo(object obj)
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
                throw new ArgumentException("");
        }
    }




    public enum SkillGroup
    {
        Hardskill,
        Softskill,
    }
}
