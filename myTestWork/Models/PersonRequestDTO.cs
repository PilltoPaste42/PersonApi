namespace myTestWork.Models
{
    public class PersonRequestDTO
    {
        public long PersonID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<SkillRequestDTO> Skills { get; set; }
    }
}
