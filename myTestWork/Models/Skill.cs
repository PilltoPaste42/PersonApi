using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myTestWork.Models
{
    public class Skill
    {
        [Key]
        [DefaultValue(null)]
        public long? SkillID { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 10, ErrorMessage = "Level must be between 1 and 10")]
        public byte Level { get; set; }

        [DefaultValue(null)]
        public long? PersonID { get; set; }
        
        
        [ForeignKey("PersonID")]
        [DefaultValue(null)]
        public virtual Person? Person { get; set; }
    }
}
