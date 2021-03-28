using MandobX.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Authentication
{
    /// <summary>
    /// Driver Class
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Driver Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// Drivers Points
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// driver vehicle
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// driver vehicle
        /// </summary>
        public Vehicle  Vehicle { get; set; }

        /// <summary>
        /// driver shipment operations
        /// </summary>
        public List<ShipmentOperation> ShipmentOperations { get; set; }

        /// <summary>
        /// driver user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// driver user
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// longitude of the Driver place
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude of the Driver place
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Driver first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Driver last name
        /// </summary>
        public string LastName { get; set; }

    }
}
