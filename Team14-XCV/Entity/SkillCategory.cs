using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace XCV.Data
{
    public class SkillCategory : SkilTreeNode, IComparable
    {
        [MaxLength(40, ErrorMessage = "Der Name der Kategorie darf 40 Zeichen nicht überschreiten.")]
        public override string Name { get; set; } = "";


        public SkillCategory Parent { get; set; }

        public List<SkilTreeNode> Children { get; set; } = new List<SkilTreeNode>();


        public SkillCategory GetRoot()
        {
            if (Parent == null)
                return this;
            else
                return Parent.GetRoot();
        }


        public override string ToString() => ToString("");  // will print the Name  with all Children Category/Skill
        public string ToString(string indentation)
        {
            if (Children.First() is Skill)
                return $"{indentation}({Name}):   " + string.Join(", ", Children.Select(x => $"[{x}-{((Skill)x).Level}]")) + "\n";
            else if (Children.First() is SkillCategory)
            {
                string subTree = $"{indentation}{Name} \n";
                foreach (SkillCategory c in Children.OrderBy(x => x.Name))
                    if (c.Children.Any())
                        subTree += c.ToString(indentation + "   ");
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
                return Name == other.Name;
            return false;
        }
        public override int GetHashCode() => HashCode.Combine(Name);    //is Db-Key
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is SkillCategory other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("");
        }
    }




    public abstract class SkilTreeNode
    {
        public virtual string Name { get; set; }
    }
}
