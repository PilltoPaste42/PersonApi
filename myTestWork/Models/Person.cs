using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myTestWork.Models
{
    public class Person
    {
        [DefaultValue(null)]
        public long? PersonID { get; set; }
        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
