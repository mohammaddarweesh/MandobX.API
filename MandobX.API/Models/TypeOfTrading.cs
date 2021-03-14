using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Authentication
{
    public class TypeOfTrading
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}