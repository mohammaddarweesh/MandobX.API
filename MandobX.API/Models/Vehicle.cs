using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Models
{
    public class Vehicle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Number { get; set; }
        public int Volume { get; set; }
        public virtual CarType CarType { get; set; }
        public virtual CarBrand CarBrand { get; set; }
        public string CarTypeId { get; set; }
        public string CarBrandId { get; set; }
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}
