using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.ViewModels
{
    /// <summary>
    /// Create Vehicle view model
    /// </summary>
    public class CreateVehicleViewModel
    {
        ///<Summary>
        /// Number of the vehicle
        ///</Summary>
        [Required]
        public string Number { get; set; }

        ///<Summary>
        /// Volume of the vehicle
        ///</Summary>
        public string Volume { get; set; }

        ///<Summary>
        /// CarTypeId of the vehicle
        ///</Summary>
        [Required]
        public string CarTypeId { get; set; }

        ///<Summary>
        /// CarBrandId of the vehicle
        ///</Summary>
        public string CarBrandId { get; set; }
    }
}
