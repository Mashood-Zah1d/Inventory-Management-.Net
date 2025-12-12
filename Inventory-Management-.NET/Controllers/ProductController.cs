 using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add([FromBody] AddProductViewModel viewmodel)
        {
            var product = new Product
            {
                ProductName = viewmodel.ProductName,
                ProductDescription = viewmodel.ProductDescription,
                ProductPrice = viewmodel.ProductPrice,
                ProductQuantity = viewmodel.ProductQuantity,
                ProductCategory = viewmodel.ProductCategory,
            };

            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getProduct()
        {
            var Product = await dbContext.Products.ToListAsync();
            return Ok(Product);
            //return View(Product);
        }

        [HttpGet]

        public async Task<IActionResult> editProduct(Guid id)
        {
            var Product = await dbContext.Products.FindAsync(id);
            if(Product is null)
            {
                return NotFound("Product Not Founded");
            }
            return Ok(Product);
            //return View(Product);
        }

        [HttpPost]

        public async Task<IActionResult> editProduct([FromBody] Product viewModel)
        {
            var Product = await dbContext.Products.FindAsync(viewModel.ProductId);
            if (Product is null)
            {
                return NotFound("Product Not Founded");
            }

            Product.ProductName = viewModel.ProductName;
            Product.ProductDescription = viewModel.ProductDescription;
            Product.ProductPrice = viewModel.ProductPrice;
            Product.ProductQuantity = viewModel.ProductQuantity;
            Product.ProductCategory = viewModel.ProductCategory;

            await dbContext.SaveChangesAsync();

            return Ok(Product);
            //return View(Product);
        }

        [HttpDelete]

        public async Task<IActionResult> deleteProduct ([FromBody] Product viewModel)
        {
                var product = await dbContext.Products.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.ProductId == viewModel.ProductId);

                if(product is null)
                {
                    return StatusCode(400, "Product Not Founded");
                }

                 dbContext.Products.Remove(viewModel);
                await dbContext.SaveChangesAsync();
                return StatusCode(200, "Product Deleted SuccessFully");

            }
            
         

    }
}
