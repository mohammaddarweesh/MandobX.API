using MandobX.API.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class UploadedFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public FileType FileType { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string FilePath { get; set; }

    }
}