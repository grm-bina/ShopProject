using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Picture
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public bool IsMainPicture { get; set; }
        [Required]
        public Product ProductID { get; set; }
    }
}
