using InventorySystem.DataAccess.Data;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand brand)
        {
            var brandFromDb = _db.Brands.FirstOrDefault(c => c.Id == brand.Id);
            if (brandFromDb != null)
            {
                brandFromDb.BrandName = brand.BrandName;
                brandFromDb.Description = brand.Description;
                brandFromDb.Status = brand.Status;
                _db.SaveChanges();
            }
        }
    }
}
