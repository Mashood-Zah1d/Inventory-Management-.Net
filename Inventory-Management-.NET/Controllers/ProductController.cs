using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductServices productService;

        public ProductController(ProductServices productService)
        {
            this.productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add( AddProductViewModel viewmodel)
        {

            var Dto = new AddProductDto
            {
                ProductName = viewmodel.ProductName,
                ProductDescription = viewmodel.ProductDescription,
                ProductPrice = viewmodel.ProductPrice,
                ProductQuantity = viewmodel.ProductQuantity,
                ProductCategory = viewmodel.ProductCategory,
            };

            await productService.AddProductAsync(Dto);
            return RedirectToAction("getProduct","Product");
        }

        [HttpGet]
        
        public async Task<IActionResult> getProduct()
        {
            var Product = await productService.GetAllProductAsync();
            return View(Product);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> editProduct(Guid id)
        {
            var Product = await productService.FindProduct(id);
            if(Product is null)
            {
                return NotFound("Product Not Founded");
            }

            var product = new AddProductViewModel
            {
                ProductId = Product.ProductId,
                ProductName= Product.ProductName,
                ProductCategory = Product.ProductCategory,
                ProductDescription= Product.ProductDescription,
                ProductPrice=Product.ProductPrice,
                ProductQuantity=Product.ProductQuantity
            };


            return View(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> editProduct(Product viewModel)
        {

            var dto = new EditProductDto
            {
                ProductName = viewModel.ProductName,
                ProductCategory = viewModel.ProductCategory,
                ProductDescription = viewModel.ProductDescription,
                ProductPrice = viewModel.ProductPrice,
                ProductQuantity = viewModel.ProductQuantity
            };

            await productService.EditProductAsync(viewModel.ProductId,dto);
            return RedirectToAction("getProduct", "Product");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> deleteProduct (Guid id)
        {
            await productService.DeleteProductAsync(id);
            return RedirectToAction("getProduct", "Product");

        }
           

    }
}
