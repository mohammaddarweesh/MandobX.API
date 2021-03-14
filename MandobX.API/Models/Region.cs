
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class Region
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public string CityId { get; set; }
    }
}
