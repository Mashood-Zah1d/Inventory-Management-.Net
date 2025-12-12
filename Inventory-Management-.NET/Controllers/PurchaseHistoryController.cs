using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Controllers
{
  
    public class PurchaseHistoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PurchaseHistoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> productOption()
        {
            var product = await dbContext.Products.ToListAsync();

            ViewBag.ProductOption = product;

            return Ok(product);

            //return View();
        } 

        [HttpPost]

        public async Task<IActionResult> Add([FromBody] PurchaseHistoryViewModel viewModel)
        {
            var product = await dbContext.Products.FindAsync(viewModel.ProductId);
            if(product is null)
            {
                return StatusCode(404, "Product Not Found");
            }
            int quantity = viewModel.purchaseQuantity + Convert.ToInt32(product.ProductQuantity);
            var purchase = new purchaseHistory
            {
                ProductId = viewModel.ProductId,
                purchaseQuantity = viewModel.purchaseQuantity,
                PurchaseDate = viewModel.PurchaseDate,
                TotalPrice = viewModel.TotalPrice,
            };
            product.ProductQuantity = quantity;
            await dbContext.purchaseHistories.AddAsync(purchase);
            await dbContext.SaveChangesAsync();

            return StatusCode(200, "Purchase Added SuccessFully");
            //return View();

        }


        
    }
}
