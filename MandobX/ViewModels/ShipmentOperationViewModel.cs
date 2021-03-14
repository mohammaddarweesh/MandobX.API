using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.ViewModels
{
    public class ShipmentOperationViewModel
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
}
