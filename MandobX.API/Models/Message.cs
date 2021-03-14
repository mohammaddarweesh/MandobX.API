using MandobX.API.Authentication;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public virtual ApplicationUser From{ get; set; }
        public virtual ApplicationUser To{ get; set; }
        public string FromId{ get; set; }
        public string ToId{ get; set; }
        public string SendDate { get; set; }
        public bool Read { get; set; }
    }
}
