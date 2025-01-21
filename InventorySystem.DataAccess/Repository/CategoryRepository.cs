using InventorySystem.DataAccess.Data;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category category)
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryFromDb != null)
            {
                categoryFromDb.CatName = category.CatName;
                categoryFromDb.Status = category.Status;
                _db.SaveChanges();
            }
        }
    }
}
