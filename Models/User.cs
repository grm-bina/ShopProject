using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.CustomAttributes;



namespace Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage ="Please enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Uploaded Products")]
        public virtual ICollection<Product> UploadedProducts { get; set; }
    }
}
