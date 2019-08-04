using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.ComponentModel.DataAnnotations;



namespace DAL.Repositories
{
    public class ProductRepository : AbstractRepository<Product, int>
    {
        public ProductRepository(ShopContext context) : base(context) { }

        public override void DeleteItem(int itemId)
        {
            Product product = _context.Products.FirstOrDefault(i => i.ID == itemId);
            if (product == null)
                throw new NullReferenceException("Product not found");
            if (product.IsSold)
                throw new InvalidOperationException("Can't remove the sold product");

            _context.Products.Remove(product);
        } 

        public override Product GetItemByID(int itemId)
        {
            Product item;
            item = _context.Products.FirstOrDefault(i => i.ID == itemId);
            item.Pictures.ToList();
            return item;
        }

        public override IEnumerable<Product> GetItems()
        {
            var items = _context.Products.ToList();
            foreach (var item in items)
                item.Pictures.ToList();
            return items;
        }

        public IEnumerable<Product> GetItems(IEnumerable<int> itemsId)
        {
            if (itemsId == null)
                return null;
            var items = _context.Products.Where(i => itemsId.Contains(i.ID)).ToList();
            foreach (var item in items)
                item.Pictures.ToList();
            return items;
        }

        public IEnumerable<Product> GetAviableItems(IEnumerable<int> notAviableItemsId)
        {
            List<Product> items;
            if(notAviableItemsId==null)
                items = _context.Products.Where(i => !i.IsSold).ToList();
            else
                items = _context.Products.Where(i => !i.IsSold && !notAviableItemsId.Contains(i.ID)).ToList();
            foreach (var item in items)
                item.Pictures.ToList();
            return items;
        }

        public override void InsertItem(Product item)
        {
            if (Validator.TryValidateObject(item, new ValidationContext(item), null))
                _context.Products.Add(item);
            else
                throw new InvalidOperationException("Invalid Data");
        }

        public override void UpdateItem(Product item)
        {
            Product current = _context.Products.FirstOrDefault(i => i.ID == item.ID);
            if (current == null)
                throw new NullReferenceException("Product not found");
            else if (!Validator.TryValidateObject(item, new ValidationContext(item), null))
                throw new InvalidOperationException("Invalid Data");
            else
            {
                current.Description = item.Description;
                current.IsSold = item.IsSold;
                current.Name = item.Name;
                current.Price = item.Price;
                current.PublicationDate = item.PublicationDate;
                current.Pictures = item.Pictures;
            }
        }
    }
}
