using MandobX.API.Authentication;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class Notification
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public ContentType ContentType { get; set; }
        public string Content { get; set; }
        public string NotificationTime { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
