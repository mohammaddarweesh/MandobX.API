using System.ComponentModel.DataAnnotations;

namespace MandobX.API.ViewModels
{
    /// <summary>
    /// login model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// user name
        /// </summary>
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// phone number
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
