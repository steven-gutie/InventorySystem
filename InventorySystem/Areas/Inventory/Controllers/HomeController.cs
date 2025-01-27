using System.Diagnostics;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Models.ErrorViewModels;
using InventorySystem.Models.Specs;
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

        public IActionResult Index(int pageNumber = 1, string search = "", string currentSearch="")
        {
            if (!String.IsNullOrEmpty(search))
            {
                pageNumber = 1;
            }
            else
            {
                search = currentSearch;
            }
            ViewData["CurrentSearch"] = search;

            if (pageNumber < 1) pageNumber = 1;

            Parameters parameters = new Parameters
            {
                PageNumber = pageNumber,
                PageSize = 8
            };
            var productList= _workUnit.Product.GetAllPaginated(parameters);
            if (!String.IsNullOrEmpty(search))
            {
                productList = _workUnit.Product.GetAllPaginated(parameters, a => a.ProdDescription.Contains(search));
            }

            ViewData["TotalPages"] = productList.MetaData.TotalPages;
            ViewData["TotalRecords"] = productList.MetaData.TotalCount;
            ViewData["PageSize"] = productList.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previous"] = "disabled";
            ViewData["Next"] = "";

            if (pageNumber > 1) ViewData["Previous"] = "";
            if(productList.MetaData.TotalPages <= pageNumber) ViewData["Next"] = "disabled";

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
