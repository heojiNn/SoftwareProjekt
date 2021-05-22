using System.ComponentModel.DataAnnotations;

namespace Team14.Data
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public SkillCatgeory Skilltype { get; set; }

        public enum SkillCatgeory
        {
            Hardskill,
            Softskill,
        }

    }
}
