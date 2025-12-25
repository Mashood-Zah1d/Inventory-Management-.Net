using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Services
{
    public class ProductServices
    {
        private readonly ApplicationDbContext dbContext;

        public ProductServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddProductAsync(AddProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                ProductDescription = dto.ProductDescription,
                ProductPrice = dto.ProductPrice,
                ProductQuantity = dto.ProductQuantity,
                ProductCategory = dto.ProductCategory
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<GetProductDto>> GetAllProductAsync()
        {
            return await dbContext.Products
                .Select(p=> new GetProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductPrice = p.ProductPrice,
                    ProductQuantity = p.ProductQuantity,
                    ProductCategory = p.ProductCategory
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
                ProductCategory = p.ProductCategory
            };
        }

        public async Task EditProductAsync(Guid id,EditProductDto dto)
        {
            var product = await dbContext.Products.FindAsync(id);

            product.ProductName = dto.ProductName;
            product.ProductDescription = dto.ProductDescription;
            product.ProductPrice = dto.ProductPrice;
            product.ProductQuantity = dto.ProductQuantity;
            product.ProductCategory = dto.ProductCategory;

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
