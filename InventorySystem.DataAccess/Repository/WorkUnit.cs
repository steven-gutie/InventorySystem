using InventorySystem.DataAccess.Data;
using InventorySystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository
{
    public class WorkUnit : IWorkUnit
    {
        private readonly ApplicationDbContext _db;
        public IStoreRepository Store { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IProductRepository Product { get; private set; }
        public IAppUserRepository AppUser { get; private set; }
        public WorkUnit(ApplicationDbContext db)
        {
            _db = db;
            Store = new StoreRepository(_db);
            Category = new CategoryRepository(_db);
            Brand = new BrandRepository(_db);
            Product = new ProductRepository(_db);
            AppUser = new AppUserRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
