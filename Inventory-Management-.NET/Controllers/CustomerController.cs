using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_.NET.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerServices customerServices;

        public CustomerController(CustomerServices customerServices)
        {
            this.customerServices = customerServices;
        }

        public async Task<IActionResult> ViewCustomer()
        {
            var customers = await customerServices.GetAllCustomersAsync();
            return View(customers); // Returns to a view that accepts List<CustomerDetailDto>
        }
        
    }
}
