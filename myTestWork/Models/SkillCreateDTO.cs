using System.ComponentModel.DataAnnotations;

namespace myTestWork.Models
{
    public class SkillCreateDTO
    {
        public string Name { get; set; }
        [Range(1, 10, ErrorMessage = "Level must be between 1 and 10")]
        public byte Level { get; set; }
    }
}
