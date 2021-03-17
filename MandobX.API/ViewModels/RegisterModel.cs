using System.ComponentModel.DataAnnotations;

namespace MandobX.API.ViewModels
{
    public class RegisterModel
    {
        /// <summary>
        /// user name 
        /// </summary>
        [Required(ErrorMessage ="UserName is required")]
        public string UserName { get; set; }

        /// <summary>
        /// password 
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// confirm password 
        /// </summary>
        [Required(ErrorMessage = "ConfirmPassword is required")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// email address
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        /// <summary>
        /// phone number
        /// </summary>
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

    }
}
