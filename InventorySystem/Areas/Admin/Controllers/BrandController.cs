using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Drawing.Drawing2D;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller 
    {
        private readonly IWorkUnit _workUnit;

        public BrandController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Brand brand = new Brand();
            if (id == null)
            {
                return View(brand);
            }
            brand = await _workUnit.Brand.Get(id.GetValueOrDefault());
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.Id == 0)
                {
                    await _workUnit.Brand.Add(brand);
                    TempData[StaticDef.Succesful] = "Brand added successfully";
                }
                else
                {
                    _workUnit.Brand.Update(brand);
                    TempData[StaticDef.Succesful] = "Brand updated successfully";
                }
                await _workUnit.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[StaticDef.Error] = "Error while creating or updating brand";
            return View(brand);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _workUnit.Brand.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _workUnit.Brand.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting this brand" });
            }
            _workUnit.Brand.Remove(objFromDb);
            await _workUnit.Save();
            return Json(new { success = true, message = "Category successfully deleted" });
        }

        [ActionName("ValidateSerialNumber")]
        public async Task<IActionResult> ValidateSerialNumber(string serialNumber, int id = 0)
        {
            bool value = false;
            var objFromDb = await _workUnit.Product.GetAll();
            if (id == 0)
            {
                value = objFromDb.Any(u => u.SerialNumber.ToLower().Trim() == serialNumber.ToLower().Trim());
            }
            else
            {
                value = objFromDb.Any(u => u.SerialNumber.ToLower().Trim() == serialNumber.ToLower().Trim() && u.Id != id);
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
