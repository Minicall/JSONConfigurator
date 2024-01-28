using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSONConfigurator.Models
{
    [Table("Configurations")]
    public class Configuration
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public List<Configuration> Children { get; set; }
        public Configuration Parent { get; set; }
    }
}
