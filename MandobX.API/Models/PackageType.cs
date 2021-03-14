using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class PackageType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}