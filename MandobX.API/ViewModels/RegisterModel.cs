using System.ComponentModel.DataAnnotations;

namespace MandobX.API.ViewModels
{
    /// <summary>
    /// Register Model
    /// </summary>
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
        [Required(ErrorMessage = "Confirm Password is required")]
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
        [RegularExpression(pattern:"^((\\+|00)?968)?[279]\\d{7}$", ErrorMessage ="please provide a valid phone number starting  with 00968 or +968 with 8 more digits starting with 2,7 or 9")]
        public string PhoneNumber { get; set; }

    }
}
