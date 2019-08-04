using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL.Repositories
{
    public abstract class AbstractRepository<T, M> : IRepository<T, M>
    {
        protected ShopContext _context;
        public AbstractRepository(ShopContext context)
        {
            _context = context;
        }

        public abstract void DeleteItem(M itemId);

        public abstract T GetItemByID(M itemId);

        public abstract IEnumerable<T> GetItems();

        public abstract void InsertItem(T item);

        public void Save()
        {
            _context.SaveChanges();
        }

        public abstract void UpdateItem(T item);

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

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
