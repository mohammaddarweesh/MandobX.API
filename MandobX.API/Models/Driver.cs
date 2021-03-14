using MandobX.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Authentication
{
    public class Driver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Points { get; set; }
        public string VehicleId { get; set; }
        public Vehicle  Vehicle { get; set; }
        public List<ShipmentOperation> ShipmentOperations { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


    }
}
