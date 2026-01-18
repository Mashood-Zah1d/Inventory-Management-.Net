using System.Diagnostics;
using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashBoardService dashBoardService;
        private readonly ProductServices productService;

        public HomeController(DashBoardService dashBoardService,ProductServices productService)
        {
            this.dashBoardService = dashBoardService;
            this.productService = productService;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var dto = await dashBoardService.getDashboardStatsASync();
            ViewBag.IsLoggedIn = Request.Cookies.ContainsKey("JwtToken");
            var categories = await productService.GetCategoryAsync();
            var categoryMap = categories.ToDictionary(c => c.categoryId, c => c.categoryName);

            var vm = new HomeViewModel
            {
                productCount = dto.productCount,
                customerCount = dto.customerCount,
                purchaseCount = dto.purchaseCount,
                orderCount = dto.orderCount,
                products = dto.products
            };

            ViewBag.CategoryMap = categoryMap; 

            return View(vm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
