namespace MandobX.API.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DriverCreateShipmentViewModel
    {
        /// <summary>
        /// Driver Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Drivers Points
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// driver vehicle
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VehicleType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VehicleBrand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VehicleNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VehicleVolume { get; set; }

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