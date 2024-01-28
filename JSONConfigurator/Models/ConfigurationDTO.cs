namespace JSONConfigurator.Models
{
    public class ConfigurationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public List<ConfigurationDTO> Children { get; set; }
    }

}
