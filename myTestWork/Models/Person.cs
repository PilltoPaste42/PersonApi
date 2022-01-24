using System.ComponentModel.DataAnnotations;

namespace myTestWork.Models
{
    public class Person
    {
        public long PersonID { get; set; }
        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
