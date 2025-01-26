using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository.IRepository
{
    public interface IWorkUnit : IDisposable
    {
        IStoreRepository Store { get; }
        ICategoryRepository Category { get; }
        IBrandRepository Brand { get; }
        IProductRepository Product { get; }
        IAppUserRepository AppUser { get; }
        Task Save();   
    }
}
