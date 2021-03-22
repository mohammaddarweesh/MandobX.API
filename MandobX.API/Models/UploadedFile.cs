using MandobX.API.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    /// <summary>
    /// uploaded files
    /// </summary>
    public class UploadedFile
    {
        /// <summary>
        /// id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// file type(personal photo-car-liecence)
        /// </summary>
        public FileType FileType { get; set; }
        /// <summary>
        /// trader or driver
        /// </summary>
        public virtual ApplicationUser User { get; set; }
        /// <summary>
        /// user id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// will be decided if base 64 or file path
        /// </summary>
        public string FilePath { get; set; }

    }
}