using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Controllers
{
    public class PurchaseHistoryController : Controller
    {
        private readonly PurchaseHistoryService purchaseHistoryService;
        private readonly ProductServices productService;

        public PurchaseHistoryController(PurchaseHistoryService purchaseHistoryService , ProductServices productService)
        {
            this.purchaseHistoryService = purchaseHistoryService;
            this.productService = productService;
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.ProductOption = await productService.GetAllProductAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PurchaseHistoryViewModel viewModel)
        {
            var result = await purchaseHistoryService.AddPurchaseAsync(viewModel);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return RedirectToAction("index", "Home");
        }
    }
}
