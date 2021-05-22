using System.ComponentModel.DataAnnotations;

namespace Team14.Data
{
    public class Skill
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(10,ErrorMessage ="maximal 10 Zeichen erlaubt")]
        public string Name { get; set; }
        public SkillCatgeory Skilltype { get; set; }

        public enum SkillCatgeory
        {
            Hardskill,
            Softskill,
        }

    }
}
