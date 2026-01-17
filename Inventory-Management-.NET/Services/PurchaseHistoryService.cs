using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Utils;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_.NET.Services
{
    public class PurchaseHistoryService
    {
        private readonly ApplicationDbContext dbContext;

        public PurchaseHistoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ResponseMessage<purchaseHistory>> AddPurchaseAsync(PurchaseHistoryViewModel viewModel)
        {
            var product = await dbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == viewModel.ProductId);

            if (product == null)
            {
                return new ResponseMessage<purchaseHistory>
                {
                    Success = false,
                    Message = "Product not found"
                };
            }

            // Update quantity
            product.ProductQuantity += viewModel.purchaseQuantity;

            // Create entity
            var purchase = new purchaseHistory
            {
                ProductId = viewModel.ProductId,
                purchaseQuantity = viewModel.purchaseQuantity,
                PurchaseDate = viewModel.PurchaseDate,
                TotalPrice = viewModel.TotalPrice
            };

            await dbContext.purchaseHistories.AddAsync(purchase);
            await dbContext.SaveChangesAsync();

            return new ResponseMessage<purchaseHistory>
            {
                Success = true,
                Message = "Purchase added successfully"
            };
        }
    }
}
