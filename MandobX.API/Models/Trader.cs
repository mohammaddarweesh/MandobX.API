using MandobX.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Authentication
{
    public class Trader
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Points { get; set; }
        public List<ShipmentOperation> ShipmentOperations { get; set; }
        public TypeOfTrading TypeOftrading{ get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
