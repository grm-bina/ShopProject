using Models.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public class PicturesViewModel
    {
        [DisplayName("Upload Main Picture")]
        [FileType(new string[] { "jpg", "jpeg", "png", "gif" }, true, ErrorMessage = "Please upload the main product's picture")]
        //[DataType(DataType.Upload)]
        public HttpPostedFileBase MainImage { get; set; }

        [DisplayName("Upload Picture")]
        [FileType(new string[] { "jpg", "jpeg", "png", "gif" }, false)]
        //[DataType(DataType.Upload)]
        public HttpPostedFileBase Image2 { get; set; }

        [DisplayName("Upload Picture")]
        [FileType(new string[] { "jpg", "jpeg", "png", "gif" }, false)]
        //[DataType(DataType.Upload)]
        public HttpPostedFileBase Image3 { get; set; }
    }
}
