using MandobX.API.Authentication;
using MandobX.API.Models;

namespace MandobX.API.Data
{
    public class OfferApplicationUser
    {
        public string OfferId { get; set; }
        public Offer Offer { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}