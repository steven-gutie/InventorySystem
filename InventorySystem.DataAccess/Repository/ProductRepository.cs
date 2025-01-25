using InventorySystem.DataAccess.Data;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventorySystem.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == "Category")
            {
                return _db.Categories.Where(x => x.Status == true).Select(i => new SelectListItem
                {
                    Text = i.CatName,
                    Value = i.Id.ToString()
                });
            }
            if (obj == "Brand")
            {
                return _db.Brands.Where(x => x.Status == true).Select(i => new SelectListItem
                {
                    Text = i.BrandName,
                    Value = i.Id.ToString()
                });
            }
            if (obj == "Product")
            {
                return _db.Products.Where(x => x.Status == true).Select(i => new SelectListItem
                {
                    Text = i.ProdDescription,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product product)
        {
            var prodFromDb = _db.Products.FirstOrDefault(c => c.Id == product.Id);
            if (prodFromDb != null)
            {
                if(prodFromDb.ImageUrl != null)
                {
                    prodFromDb.ImageUrl = product.ImageUrl;
                }
                prodFromDb.SerialNumber = product.SerialNumber;
                prodFromDb.ProdDescription = product.ProdDescription;
                prodFromDb.Price = product.Price;
                prodFromDb.Cost = product.Cost;
                prodFromDb.Status = product.Status;
                prodFromDb.CategoryId = product.CategoryId;
                prodFromDb.BrandId = product.BrandId;
                prodFromDb.ParentId = product.ParentId;
                _db.SaveChanges();
            }
        }
    }
}
