using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Models.ViewModels;
using InventorySystem.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWorkUnit _workUnit;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IWorkUnit workUnit, IWebHostEnvironment hostEnvironment)
        {
            _workUnit = workUnit;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM productVm = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _workUnit.Product.GetAllDropdownList("Category"),
                BrandList = _workUnit.Product.GetAllDropdownList("Brand"),
                ParentList = _workUnit.Product.GetAllDropdownList("Product")
            };
            if (id == null)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product = await _workUnit.Product.Get(id.GetValueOrDefault());
                if (productVm.Product == null)
                {
                    return NotFound();
                }
                return View(productVm);
             }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVm)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (productVm.Product.Id == 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = webRootPath + StaticDef.ImagePath;
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productVm.Product.ImageUrl = fileName + extension;
                    await _workUnit.Product.Add(productVm.Product);
                }
                else
                {
                    var objFromDb = await _workUnit.Product.GetFirstOrDefault(x => x.Id == productVm.Product.Id, isTracking: false);
                    if (files.Count > 0) // This verifies if a new image was selected
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, StaticDef.ImagePath);
                        var extension_new = Path.GetExtension(files[0].FileName);
                        var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath); // This deletes the current image
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        productVm.Product.ImageUrl = fileName + extension_new;
                    }
                    else
                    {
                        productVm.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                    _workUnit.Product.Update(productVm.Product);
                }
                TempData[StaticDef.Succesful] = "Product saved successfully";
                await _workUnit.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVm.CategoryList = _workUnit.Product.GetAllDropdownList("Category");
                productVm.BrandList = _workUnit.Product.GetAllDropdownList("Brand");
                productVm.ParentList = _workUnit.Product.GetAllDropdownList("Product");
                return View(productVm);
            }
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _workUnit.Product.GetAll(includeProps:"Category,Brand");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _workUnit.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting product" });
            }
            // Delete the image
            var upload = _hostEnvironment.WebRootPath + StaticDef.ImagePath;
            var imagePath = Path.Combine(upload, objFromDb.ImageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _workUnit.Product.Remove(objFromDb);
            await _workUnit.Save();
            return Json(new { success = true, message = "Product deleted successfully" });
        }

        [ActionName("ValidateBrandName")]
        public async Task<IActionResult> ValidateBrandName(string brandName, int id = 0)
        {
            bool value = false;
            var objFromDb = await _workUnit.Brand.GetAll();
            if (id == 0)
            {
                value = objFromDb.Any(u => u.BrandName.ToLower().Trim() == brandName.ToLower().Trim());
            }
            else
            {
                value = objFromDb.Any(u => u.BrandName.ToLower().Trim() == brandName.ToLower().Trim() && u.Id != id);
            }
            if (value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByCategory(int id)
        {
            var allObj = await _workUnit.Product.GetAll(u => u.CategoryId == id);
            return Json(new { data = allObj });
        }
        [HttpPost]
        public async Task<IActionResult> GetAllByBrand(int id)
        {
            var allObj = await _workUnit.Product.GetAll(u => u.BrandId == id);
            return Json(new { data = allObj });
        }
        #endregion
    }
}
