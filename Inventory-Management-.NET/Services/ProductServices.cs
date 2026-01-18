using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Services
{
    public class ProductServices
    {
        private readonly ApplicationDbContext dbContext;
        private readonly CloudinaryService cloudinaryService;

        public ProductServices(ApplicationDbContext dbContext,CloudinaryService cloudinaryService)
        {
            this.dbContext = dbContext;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<List<CategoryDto>> GetCategoryAsync()
        {
            return await dbContext.Categories.Select(c => new CategoryDto
            {
                categoryId=c.CategoryId,
                categoryName=c.CategoryName
            }).ToListAsync();
        }

        public async Task<AddProductDto> AddProductAsync(AddProductViewModel viewmodel)
        {
            var imageUrl = await cloudinaryService.UploadImageAsync(viewmodel.Image);

            var category = await dbContext.Categories
       .FirstOrDefaultAsync(c => c.CategoryId == viewmodel.ProductCategory);




            if (category == null)
            {
                throw new Exception("Selected category does not exist.");
            }

            var product = new Product
            {
                ImageUrl = imageUrl.Data,
                ProductName = viewmodel.ProductName,
                ProductDescription = viewmodel.ProductDescription,
                ProductPrice = viewmodel.ProductPrice,
                ProductQuantity = viewmodel.ProductQuantity,
                CategoryId = category.CategoryId

            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            
   

            return new AddProductDto
            {
                Image = imageUrl.Data,
                ProductName = viewmodel.ProductName,
                ProductDescription = viewmodel.ProductDescription,
                ProductPrice = viewmodel.ProductPrice,
                ProductQuantity = viewmodel.ProductQuantity,
                ProductCategory = category.CategoryId
            };

        }


        public async Task<List<GetProductDto>> GetAllProductAsync()
        {
            return await dbContext.Products
                .Select(p=> new GetProductDto
                {

                    Image=p.ImageUrl,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductPrice = p.ProductPrice,
                    ProductQuantity = p.ProductQuantity,
                    ProductCategory = p.CategoryId
                })
                .ToListAsync();
        }

        public async Task<FindProductDto> FindProduct(Guid id)
        {
            var p = await dbContext.Products.FindAsync(id);
            return new FindProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductPrice = p.ProductPrice,
                ProductQuantity = p.ProductQuantity,
                ProductCategory = p.CategoryId
            };
        }

        public async Task EditProductAsync(Guid id,EditProductDto dto)
        {
            var product = await dbContext.Products.FindAsync(id);

            product.ProductName = dto.ProductName;
            product.ProductDescription = dto.ProductDescription;
            product.ProductPrice = dto.ProductPrice;
            product.ProductQuantity = dto.ProductQuantity;
            product.CategoryId = dto.ProductCategory;

            dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await dbContext.Products.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.ProductId == id);

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
           
        }
    }
}
