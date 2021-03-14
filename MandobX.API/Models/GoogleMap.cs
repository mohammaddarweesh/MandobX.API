
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class GoogleMap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public double Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }


    }
}
