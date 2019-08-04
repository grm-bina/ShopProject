using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;



namespace DAL.Repositories
{
    public class UserRepository : AbstractRepository<User, string>
    {
        public UserRepository(ShopContext context) : base(context) { }

        public override void DeleteItem(string itemId)
        {
            User user = _context.Users.FirstOrDefault(u => u.Username == itemId);
            if (user == null)
                throw new NullReferenceException("User not found");
            else if (user.UploadedProducts != null && user.UploadedProducts.FirstOrDefault() != null)
                throw new InvalidOperationException("The user with uploaded products can't be removed");
            else
                _context.Users.Remove(user);
        }

        public override User GetItemByID(string itemId)
        {
            return _context.Users.FirstOrDefault(u => u.Username == itemId);
        }

        public override IEnumerable<User> GetItems()
        {
            return _context.Users.ToList();
        }

        public override void InsertItem(User item)
        {

            if (!Validator.TryValidateObject(item, new ValidationContext(item) , null))
                throw new InvalidOperationException("Invalid Data");

                User check = _context.Users.FirstOrDefault(u => u.Username==item.Username);
            if (check != null)
                throw new InvalidOperationException("User is already exist");

            check = _context.Users.FirstOrDefault(u => u.Email == item.Email);

            if (check != null)
                throw new InvalidOperationException("User with this Email already exist");

            _context.Users.Add(item);
        }

        public override void UpdateItem(User item)
        {
            User current = _context.Users.FirstOrDefault(u => u.Username == item.Username);
            if (current == null)
                throw new NullReferenceException("User not found");
            else
            {
                User check = _context.Users.FirstOrDefault(u => String.Equals(u.Email, item.Email, StringComparison.InvariantCultureIgnoreCase));
                if (check != null && check.Username!=current.Username)
                    throw new InvalidOperationException("User with this Email already exist");

                if (!Validator.TryValidateObject(item, new ValidationContext(item), null))
                    throw new InvalidOperationException("Invalid Data");

                current.Email = item.Email;
                current.Password = item.Password;
                current.UploadedProducts = item.UploadedProducts;
            }
        }

        public bool AutorizationCheck(LoginViewModel model, out User currentUser)
        {
            User temp = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (temp == null || temp.Password != model.Password)
            {
                currentUser = null;
                return false;
            }
            else
            {
                currentUser = temp;
                return true;
            }
        }
    }
}
