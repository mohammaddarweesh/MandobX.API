using MandobX.API.Authentication;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class ShipmentOperation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string FromRegionId { get; set; }
        public virtual Region FromRegion { get; set; }
        public string ToRegionId { get; set; }
        public virtual Region ToRegion { get; set; }
        public string GoogleMapId { get; set; }
        public virtual GoogleMap GoogleMap { get; set; }
        public string CreationDate { get; set; }
        public string ShipmentDate { get; set; }
        public virtual Driver Driver { get; set; }
        public string? DriverId { get; set; }
        public virtual Trader Trader { get; set; }
        public string TraderId { get; set; }
        public string RecieverPhoneNumber { get; set; }
        public string RecieverName { get; set; }
        public string Packages { get; set; }
        public virtual PackageType PackageType { get; set; }
        public string PackageTypeId { get; set; }
        public int Price { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public int ToTraderCode { get; set; }
        public int ToRecieverCode { get; set; }
        public bool IsUrgent { get; set; } = false;
    }
}
