using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Models;

namespace DAL
{
    public static class PicturesSerializationAdapter
    {
        public static byte[] Serialize (HttpPostedFileBase pictureFile)
        {
            try
            {
                if (pictureFile != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureFile.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                        return array;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
