using InventorySystem.DataAccess.Data;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository
{
    public class StoreRepository : Repository<Store> , IStoreRepository
    {
        private readonly ApplicationDbContext _db;
        public StoreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Store store)
        {
            var storeFromDb = _db.Stores.FirstOrDefault(s => s.Id == store.Id);
            if (storeFromDb != null)
            {
                storeFromDb.StoreName = store.StoreName;
                storeFromDb.Description = store.Description;
                storeFromDb.Status = store.Status;
                _db.SaveChanges();
            }
        }
    }
}
