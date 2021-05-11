using System.ComponentModel.DataAnnotations;

namespace Team14.Data
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }
        public string Name { get; set; }
        public SkillCatgeory Catgeory { get; set; }

        public enum SkillCatgeory
        {
            Hardskill,
            Softskill,
        }

    }
}
