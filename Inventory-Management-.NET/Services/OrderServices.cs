using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Utils;

namespace Inventory_Management_.NET.Services
{
    public class OrderServices
    {
        private readonly ApplicationDbContext dbContext;

        public OrderServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ResponseMessage<Order>> AddDetailsAsync(Dictionary<Guid,OrderDetailDto> OrderDetail,CustomerDetailDto dto)
        {
            var customer = new Customer
            {
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                Phone = dto.Phone,
            };

            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();

            var TotalAmount = OrderDetail.Sum(x => x.Value.price * x.Value.qty);

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                OrderDate = new DateOnly(),
                TotalAmount = TotalAmount
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            foreach (var item in OrderDetail) {
                var orderdetail = new OrderDetails
                {
                    OrderId = order.OrderId,
                    ProductId = item.Value.id,
                    Quantity = item.Value.qty,
                    Price = item.Value.price,
                    TotalPrice = item.Value.qty * item.Value.price
                };

               await dbContext.orderDetails.AddAsync(orderdetail);
               await dbContext.SaveChangesAsync();
            }

            return new ResponseMessage<Order>
            {
                Success = true,
                Message = "Details Added SuccessFully",
            };
            
        }
    }
}
