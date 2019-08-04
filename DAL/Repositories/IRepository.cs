using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL.Repositories
{
    interface IRepository<T,M>:IDisposable
    {
        IEnumerable<T> GetItems();
        T GetItemByID(M itemId);
        void InsertItem(T item);
        void DeleteItem(M itemId);
        void UpdateItem(T item);
        void Save();
    }
}
