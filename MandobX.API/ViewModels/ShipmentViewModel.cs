using MandobX.API.Authentication;
using MandobX.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MandobX.API.ViewModels
{
    /// <summary>
    /// Shipment view model
    /// </summary>
    public class ShipmentViewModel
    {
        public List<Region> Regions { get; set; }
        public List<PackageType> PackageTypes { get; set; }
        public List<DriverCreateShipmentViewModel> Drivers { get; set; }
    }
    public class ShipmentListViewModel
    {
        public string Id { get; set; }
        [Display(Name = "From")]
        public string FromRegion { get; set; }
        [Display(Name = "To")]
        public string ToRegion { get; set; }
        [Display(Name = "Creation Date")]
        public string CreationDate { get; set; }
        [Display(Name = "Shipment Date")]
        public string ShipmentDate { get; set; }
        public string Driver { get; set; }
        public string Trader { get; set; }
        [Display(Name = "Reciever Phone Number")]
        public string RecieverPhoneNumber { get; set; }
        [Display(Name = "Reciever Name")]
        public string RecieverName { get; set; }
        public string Packages { get; set; }
        [Display(Name = "Package Type")]
        public string PackageType { get; set; }
        public int Price { get; set; }
        [Display(Name = "Shipment Status")]
        public string ShipmentStatus { get; set; }
        [Display(Name = "Trader Code")]
        public int ToTraderCode { get; set; }
        [Display(Name = "Reciever Code")]
        public int ToRecieverCode { get; set; }
    } 
    public class CreateShipmentViewModel
    {
        [Required(ErrorMessage = "From Region is Required")]
        public string FromRegionId { get; set; }

        [Required(ErrorMessage = "To Region is Required")]
        public string ToRegionId { get; set; }

        public string CreationDate { get; set; }

        [Required(ErrorMessage = "Shipment Date is Required")]
        public string ShipmentDate { get; set; }

        [Required(ErrorMessage = "Driver is Required")]
        public string DriverId { get; set; }

        [Required(ErrorMessage = "Trader is Required")]
        public string TraderId { get; set; }

        [Required(ErrorMessage = "Reciever Phone Number is Required")]
        public string RecieverPhoneNumber { get; set; }

        [Required(ErrorMessage = "Reciever Name is Required")]
        public string RecieverName { get; set; }

        [Required(ErrorMessage = "Packages are Required")]
        public string Packages { get; set; }

        [Required(ErrorMessage = "Package Type is Required")]
        public string PackageTypeId { get; set; }

        [Required(ErrorMessage = "Time is Required")]
        public double Time { get; set; }

        [Required(ErrorMessage = "Latitude is Required")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longtitude is Required")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Distance is Required")]
        public double Distance { get; set; }
    }

    public class EditShipmentViewModel:CreateShipmentViewModel
    {
        public string Id { get; set; }
    }
}
