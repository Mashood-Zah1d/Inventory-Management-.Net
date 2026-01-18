using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Inventory_Management_.NET.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductServices productService;
        private readonly CloudinaryService cloudinaryService;

        public ProductController(ProductServices productService,CloudinaryService cloudinaryService)
        {
            this.productService = productService;
            this.cloudinaryService = cloudinaryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add()
        {
            ViewBag.CategoryOption = await productService.GetCategoryAsync();
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddProductViewModel viewmodel)
        {
            if (!ModelState.IsValid)
                return View(viewmodel);

            var productDto = await productService.AddProductAsync(viewmodel);

            TempData["Success"] = $"{productDto.ProductName} added successfully!";
            return RedirectToAction("GetProduct", "Product");
        }


        [HttpGet]
        public async Task<IActionResult> getProduct()
        {
            var Product = await productService.GetAllProductAsync();
            var categories = await productService.GetCategoryAsync();
            ViewBag.CategoryMap = categories.ToDictionary(c => c.categoryId, c => c.categoryName);
            return View(Product);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> editProduct(Guid id)
        {
            var productDto = await productService.FindProduct(id);
            if(productDto is null)
            {
                return NotFound("Product Not Founded");
            }

            var viewModel = new AddProductViewModel
            {
                ProductId = productDto.ProductId,
                ProductName = productDto.ProductName,
                ProductDescription = productDto.ProductDescription,
                ProductPrice = productDto.ProductPrice,
                ProductQuantity = productDto.ProductQuantity,
                ProductCategory = productDto.ProductCategory
            };
            ViewBag.CategoryOption = await productService.GetCategoryAsync();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> editProduct(Product viewModel)
        {

            var dto = new EditProductDto
            {
                ProductName = viewModel.ProductName,
                ProductCategory = viewModel.CategoryId,
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
