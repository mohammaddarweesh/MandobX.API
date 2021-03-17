using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    ///<Summary>
    /// Gets the answer
    ///</Summary>
    public class Vehicle
    {
        ///<Summary>
        ///Id of the vehicle
        ///</Summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

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
        /// CarType of the vehicle
        ///</Summary>
        public virtual CarType CarType { get; set; }

        ///<Summary>
        /// CarBrand of the vehicle
        ///</Summary>
        public virtual CarBrand CarBrand { get; set; }

        ///<Summary>
        /// CarTypeId of the vehicle
        ///</Summary>
        [Required]
        public string CarTypeId { get; set; }

        ///<Summary>
        /// CarBrandId of the vehicle
        ///</Summary>
        public string CarBrandId { get; set; }

        ///<Summary>
        /// UploadedFiles of the vehicle
        ///</Summary>
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}
