using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDef.Role_Admin)]
    public class StoreController : Controller 
    {
        private readonly IWorkUnit _workUnit;

        public StoreController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Store store = new Store();
            if (id == null)
            {
                return View(store);
            }
            store = await _workUnit.Store.Get(id.GetValueOrDefault());
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Store store)
        {
            if (ModelState.IsValid)
            {
                if (store.Id == 0)
                {
                    await _workUnit.Store.Add(store);
                    TempData[StaticDef.Succesful] = "Store added successfully";
                }
                else
                {
                    _workUnit.Store.Update(store);
                    TempData[StaticDef.Succesful] = "Store updated successfully";
                }
                await _workUnit.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[StaticDef.Error] = "Error while creating or updating store";
            return View(store);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _workUnit.Store.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _workUnit.Store.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting this store" });
            }
            _workUnit.Store.Remove(objFromDb);
            await _workUnit.Save();
            return Json(new { success = true, message = "Store successfully deleted" });
        }

        [ActionName("ValidateStoreName")]
        public async Task<IActionResult> ValidateStoreName(string storeName, int id = 0)
        {
            bool value = false;
            var objFromDb = await _workUnit.Store.GetAll();
            if (id == 0)
            {
                value = objFromDb.Any(u => u.StoreName.ToLower().Trim() == storeName.ToLower().Trim());
            }
            else
            {
                value = objFromDb.Any(u => u.StoreName.ToLower().Trim() == storeName.ToLower().Trim() && u.Id != id);
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
