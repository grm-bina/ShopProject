using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using Models;


namespace DAL
{
    public class DbManager:IDisposable
    {
        private ShopContext _context = new ShopContext();
        private UserRepository _userRepository;
        private PictureRepository _pictureRepository;
        private ProductRepository _productRepository;

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public PictureRepository PictureRepository
        {
            get
            {
                if (_pictureRepository == null)
                    _pictureRepository = new PictureRepository(_context);
                return _pictureRepository;
            }
        }

        public ProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public Respone DeleteUser(string username)
        {
            try
            {
                UserRepository.DeleteItem(username);
                Save();
                return new Respone { IsDone = true };
            }
            catch(Exception e)
            {
                Rollback();
                return new Respone { IsDone = false, Message=e.Message };
            }
        }

        public Respone AddUser(User user)
        {
            try
            {
                UserRepository.InsertItem(user);
                Save();
                return new Respone { IsDone = true };
            }
            catch (Exception e)
            {
                Rollback();
                return new Respone { IsDone = false, Message = e.Message };
            }
        }

        public Respone AddProduct(ProductCreateViewModel product, string username)
        {
            User current = UserRepository.GetItemByID(username);
            if (current == null)
                return new Respone { IsDone = false, Message = "Only signed users can add products" };
            else
            {
                try
                {
                    product.Product.PublicationDate = DateTime.Now;
                    product.Product.UploadedByUser = current;
                    if (Validator.TryValidateObject(product.Files, new ValidationContext(product.Files), null))
                    {
                        ProductRepository.InsertItem(product.Product);
                        PictureRepository.InsertItem(new Picture() { IsMainPicture = true, ProductID = product.Product, Image = PicturesSerializationAdapter.Serialize(product.Files.MainImage) });
                        PictureRepository.InsertItem(new Picture() { ProductID = product.Product, Image = PicturesSerializationAdapter.Serialize(product.Files.Image2) });
                        PictureRepository.InsertItem(new Picture() { ProductID = product.Product, Image = PicturesSerializationAdapter.Serialize(product.Files.Image3) });
                        Save();
                        return new Respone { IsDone = true };

                    }
                    else
                        throw new Exception("Invalid files");
                }
                catch (Exception e)
                {
                    Rollback();
                    return new Respone { IsDone = false, Message = e.Message };
                }
            }
        }

        public Respone DeleteProduct(int productId)
        {
            try
            {
                var pictures = ProductRepository.GetItemByID(productId).Pictures;
                ProductRepository.DeleteItem(productId);
                foreach (var pic in pictures)
                {
                    PictureRepository.DeleteItem(pic.ID);
                }
                Save();
                return new Respone { IsDone = true };
            }
            catch (Exception e)
            {
                Rollback();
                return new Respone { IsDone = false, Message = e.Message };
            }
        }

        public void Rollback()
        {
            var changed = _context.ChangeTracker.Entries().Where(e => e.State != System.Data.Entity.EntityState.Unchanged);
            foreach (var item in changed)
            {
                item.State = System.Data.Entity.EntityState.Unchanged;
            }
        }


        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _userRepository.Dispose();
                    _pictureRepository.Dispose();
                    _productRepository.Dispose();
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
