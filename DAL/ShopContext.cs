using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Models;

namespace DAL
{
    public class ShopContext : DbContext
    {
        public ShopContext() : base("AlbinaDB") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Picture> ProductsPictures { get; set; }
    }
}
