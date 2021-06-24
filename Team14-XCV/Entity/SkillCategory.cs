using System;
using System.Collections.Generic;
using System.Linq;


namespace XCV.Data
{
    public class SkillCategory : IComparable
    {
        public string Name { get; set; } = "";


        public SkillCategory Parent { get; set; }

        public List<SkillCategory> Children { get; set; } = new List<SkillCategory>();



        public override string ToString() => ToString("");



        public string ToString(string indentation)
        {
            if (Children.First() is Skill)
                return $"{indentation}{Name}: {string.Join(", ", Children)}\n";
            else if (Children.First() is SkillCategory)
            {
                string subTree = $"{indentation}{Name} \n";
                foreach (SkillCategory c in Children.OrderBy(x => x.Name))
                    subTree += c.ToString(indentation + "\t");
                return subTree;
            }
            else
                throw new Exception("Tree Structure Error");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is SkillCategory other)
                return Parent?.Name == other.Parent?.Name && Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Parent.Name, Name);
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is SkillCategory other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Kann nur mit SkillCategorys vergleichen");
        }
    }
}
