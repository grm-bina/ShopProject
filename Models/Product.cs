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
    public class Product
    {
        [Key]
        public int ID { get; set; }

        public bool IsSold { get; set; }

        [Required(ErrorMessage ="Please enter the product's name")]
        [MinLength(2, ErrorMessage = "Too short name (enter minimum 2 characters)")]
        [MaxLength(100,ErrorMessage ="Too long name (enter maximum 100 characters)")]
        public string Name { get; set; }

        [DisplayName("Publication Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Please enter the product's description")]
        [DataType(DataType.MultilineText)]
        [MinLength(10, ErrorMessage = "Too short description (enter minimum 10 characters)")]
        [MaxLength(1000, ErrorMessage = "Too long description (enter maximum 1000 characters)")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Please enter the product's price")]
        [Range(1,(double)Decimal.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        [Required]
        [DisplayName("Uploaded By User")]
        public virtual User UploadedByUser { get; set; }
    }
}
