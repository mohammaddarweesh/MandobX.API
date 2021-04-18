using MandobX.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Authentication
{
    public class ApplicationUser : IdentityUser
    {

        public string UserType { get; set; }
        public List<Offer> Offers { get; set; }
        public List<UploadedFile> UploadedFiles { get; set; }
        public UserStatus UserStatus { get; set; }
        public bool AccountActivated { get; set; }
        public int VerificationCode { get; set; }

    }
}
