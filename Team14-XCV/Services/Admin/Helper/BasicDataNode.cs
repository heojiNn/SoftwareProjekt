using System;
using System.Collections.Generic;
using System.Linq;

namespace Team14.Data
{
    public class BasicDataNode : BasicDataLeaf
    {
        // Service checkes for not loger than 25
        public new string Name { get; init; } = ""; //set while deseriallising
        public string LongName { get; init; } = ""; //set while deseriallising

        public IEnumerable<BasicDataLeaf> Children { get; set; } = new List<BasicDataLeaf>(); //set while deseriallising

        public IEnumerable<SkillExtensionValue> Extensions { get; init; } = new List<SkillExtensionValue>(); //set while deseriallising
        public string [] LevelNames{ get; set; } // heritage set while CheckCombieFunction in Service




        public override string ToString() => ToString("");

        public string ToString(string indentation)
        {
            if (Children.First() is BasicDataNode)
            {
                string subTree = $"{indentation}{Name}\n";
                foreach (BasicDataNode c in Children.OrderBy(x => x.Name))
                    subTree += c.ToString(indentation + "\t");
                return subTree;
            }
            else if (Children.First() is BasicDataLeaf && !(Children.First() is BasicDataNode))
                return $"{indentation}{Name}: {string.Join(", ", Children)}\n";
            else
                throw new Exception("Tree Structure Error");
        }
    }


    public enum SkillExtensionValue
    {
        Periode
        // MAYBE  Bonus Features
        //  Role-with-StartDate      Framwork-Level-with-Version-Number
    }
}
