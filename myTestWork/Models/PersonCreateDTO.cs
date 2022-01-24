namespace myTestWork.Models
{
    public class PersonCreateDTO
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<SkillCreateDTO> Skills { get; set; }
    }
}
