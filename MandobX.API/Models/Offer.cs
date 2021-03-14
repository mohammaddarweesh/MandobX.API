using MandobX.API.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class Offer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Percentage { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
