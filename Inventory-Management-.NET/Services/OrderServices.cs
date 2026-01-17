using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Utils;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await (
                from o in dbContext.Orders
                join c in dbContext.Customers on o.CustomerId equals c.CustomerId
                select new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    CustomerName = c.CustomerName,
                    Email = c.Email,
                    Phone = c.Phone
                }
            ).ToListAsync();
        }

    }
}
