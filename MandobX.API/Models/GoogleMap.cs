
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    /// <summary>
    /// google map coordinates
    /// </summary>
    public class GoogleMap
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        /// <summary>
        /// time between the shipment place and the reciever
        /// </summary>
        public double Time { get; set; }
        /// <summary>
        /// Latitude of the shipment operation place
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// longitude of the shipment operation place
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// distance between the sender and the reciever
        /// </summary>
        public double Distance { get; set; }


    }
}
