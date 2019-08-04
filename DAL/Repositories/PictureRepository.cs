using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;


namespace DAL.Repositories
{
    public class PictureRepository : AbstractRepository<Picture, int>
    {
        public PictureRepository(ShopContext context) : base(context) { }

        public override void DeleteItem(int itemId)
        {
            Picture pic = _context.ProductsPictures.FirstOrDefault(i => i.ID == itemId);
            if (pic == null)
                throw new NullReferenceException("Picture not found");
            else
                _context.ProductsPictures.Remove(pic);
        }

        public override Picture GetItemByID(int itemId)
        {
            return _context.ProductsPictures.FirstOrDefault(i => i.ID == itemId);
        }

        public override IEnumerable<Picture> GetItems()
        {
            return _context.ProductsPictures.ToList();
        }

        public override void InsertItem(Picture item)
        {
            if (Validator.TryValidateObject(item, new ValidationContext(item), null))
                _context.ProductsPictures.Add(item);
            else if (item.Image == null)
                return;
            else
                throw new InvalidOperationException("Invalid Data");
        }

        public override void UpdateItem(Picture item)
        {
            Picture current = _context.ProductsPictures.FirstOrDefault(i => i.ID == item.ID);
            if (current == null)
                throw new NullReferenceException("Picture not found");
            else if (!Validator.TryValidateObject(item, new ValidationContext(item), null))
                throw new InvalidOperationException("Invalid Data");
            else
            {
                current.Image = item.Image;
                current.IsMainPicture = item.IsMainPicture;
                current.ProductID = item.ProductID;
            }
               
        }


    }
}
