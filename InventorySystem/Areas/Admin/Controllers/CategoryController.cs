using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller 
    {
        private readonly IWorkUnit _workUnit;

        public CategoryController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            category = await _workUnit.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    await _workUnit.Category.Add(category);
                    TempData[StaticDef.Succesful] = "Category added successfully";
                }
                else
                {
                    _workUnit.Category.Update(category);
                    TempData[StaticDef.Succesful] = "Category updated successfully";
                }
                await _workUnit.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[StaticDef.Error] = "Error while creating or updating category";
            return View(category);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _workUnit.Category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _workUnit.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting this category" });
            }
            _workUnit.Category.Remove(objFromDb);
            await _workUnit.Save();
            return Json(new { success = true, message = "Category successfully deleted" });
        }

        [ActionName("ValidateCategoryName")]
        public async Task<IActionResult> ValidateCategoryName(string catName, int id = 0)
        {
            bool value = false;
            var objFromDb = await _workUnit.Category.GetAll();
            if (id == 0)
            {
                value = objFromDb.Any(u => u.CatName.ToLower().Trim() == catName.ToLower().Trim());
            }
            else
            {
                value = objFromDb.Any(u => u.CatName.ToLower().Trim() == catName.ToLower().Trim() && u.Id != id);
            }
            if (value)
            {
                return Json(new {data = true});
            }
            return Json(new { data = false });
        }
        #endregion
    }
}
