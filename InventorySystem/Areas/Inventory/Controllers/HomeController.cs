using System.Diagnostics;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Models.ErrorViewModels;
using InventorySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkUnit _workUnit;

        public HomeController(ILogger<HomeController> logger, IWorkUnit workUnit)
        {
            _logger = logger;
            _workUnit = workUnit;
        }

        public async Task <IActionResult> Index()
        {
            IEnumerable<Product> productList = await _workUnit.Product.GetAll();
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
