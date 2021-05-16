using System.ComponentModel.DataAnnotations;
namespace Team14.Data
{
    public class Skill
    {
        public Skill()
        {
        }

        [Key]
        public int SkillId { get; }
        public string Name { get; set; }
        public SkillKategorie Kategorisierung { get; set; }

        public enum SkillKategorie
        {
            Hardskill,
            Softskill,
        }

    }
}
