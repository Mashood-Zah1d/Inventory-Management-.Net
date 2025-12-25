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

        public HomeController(DashBoardService dashBoardService)
        {
            this.dashBoardService = dashBoardService;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var dto = await dashBoardService.getDashboardStatsASync();
            var vm = new HomeViewModel
            {
                productCount = dto.productCount,
                customerCount = dto.customerCount,
                purchaseCount = dto.purchaseCount,
                orderCount = dto.orderCount,
                products = dto.products

            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
