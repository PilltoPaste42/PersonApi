using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myTestWork.Models
{
    public class Skill
    {
        [Key]
        public long SkillID { get; set; }
        [Required]
        public string Name { get; set; }
        public byte Level { get; set; }
        public long PersonID { get; set; }
        [ForeignKey("PersonID")]
        public virtual Person Person { get; set; }
    }
}
