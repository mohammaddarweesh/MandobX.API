using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.ViewModels
{
    /// <summary>
    /// Edit profile View Model
    /// </summary>
    public class EditProfileViewModel
    {
        /// <summary>
        /// trader or driver id
        /// </summary>
        public string Id { get; set; }

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

        /// <summary>
        /// Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
