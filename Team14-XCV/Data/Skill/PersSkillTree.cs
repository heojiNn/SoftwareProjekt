using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Team14.Data
{
    public class PersSkillTree
    {
        // implementetion of
        // ISkillData
        public string Name { get; init; }

        // is usally equal to Name 
        // except
        //  if there existst another one with the same(case insesetive) name
        //  then its  "parent Name+Name"  that has to be uniqe because primaryKey
        [JsonIgnore]
        public string Identifier { get; init; }

        [JsonIgnore]
        public PersSkillTree Parent { get; init; }

        public bool hasTwin => Name.Equals(Identifier);


        // implementetion of
        // ISkillCategory
        public string LongName { get; init; }

        [JsonIgnore]
        public IEnumerable<PersSkillTree> Children
        {
            get;
            set;
        }


        // min and max  are in nearly all cases  0
        [JsonPropertyName("min")]
        public int MinMultiplicty { get; init; }
        // Special Value 0==infinity
        [JsonPropertyName("max")]
        public int MaxMultiplicty { get; init; }

        // and so these both are usally ture
        public bool isOptional => MinMultiplicty == 0;
        public bool hasNoLimit => MaxMultiplicty == 0;

        public bool isRadio => MaxMultiplicty == 1;



        public IEnumerable<string> LevelNames { get; init; }
        public string ValueName { get; init; }



        public override string ToString() => ToString("");

        public string ToString(string indentation)
        {
            if (Children.First() is PersSkillTree)
            {
                string subTree = $"{indentation}{Identifier}\n";
                foreach (PersSkillTree c in Children.OrderBy(loTree => loTree.Identifier))
                    subTree += c.ToString(indentation + "\t");
                return subTree;
            }
            //else if (Children.First() is ISkillData && !(Children.First() is ISkillCategory))
            //   return $"{indentation}{Identifier}: {string.Join(", ", Children)}\n";
            else
                throw new Exception("Tree Structure Error");
        }

    }
}
