using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Inventory_Management_.NET.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProductServices productServices;
        private readonly OrderServices orderServices;

        public OrderController(ProductServices productServices,OrderServices orderServices)
        {
            this.productServices = productServices;
            this.orderServices = orderServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProduct()
        {
            var products = await productServices.GetAllProductAsync();

            return View(products);
        }

        public async Task<IActionResult> GenerateBill()
        {

            return View();
        }

        public async Task<IActionResult> CheckOut(GetCustomerDetailViewModel vm)
        {
            // Deserialize basket JSON
            var orderDetail = JsonConvert.DeserializeObject<Dictionary<Guid, OrderDetailDto>>(vm.BasketJson);

            // Map customer data
            var customerDto = new CustomerDetailDto
            {
                CustomerName = vm.CustomerName,
                Email = vm.Email,
                Phone = vm.Phone
            };

            // Save order & details
            var result = await orderServices.AddDetailsAsync(orderDetail, customerDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(vm); // Return checkout view with errors
            }

            // Prepare view model for bill/thank-you page
            var billViewModel = new BillViewModel
            {
                Customer = customerDto,
                OrderDetails = orderDetail.Values.ToList(),
                GrandTotal = orderDetail.Sum(x => x.Value.price * x.Value.qty)
            };

            // Render ThankYou/Bill view
            return View("ThankYou", billViewModel);
        }


    }
}
