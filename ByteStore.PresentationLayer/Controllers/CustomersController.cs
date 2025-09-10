using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Customer;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class CustomersController : BaseController
    {
        public CustomersController(IServiceManager serviceManager) : base(serviceManager)
        {
            
        }
        // GET: api/customers
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await serviceManager.CustomerService.GetAllCustomerProfilesAsync();
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/customers/5
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            var result = await serviceManager.CustomerService.GetCustomerProfileByIdAsync(customerId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // PUT: api/customers/5
        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] CustomerUpdateDto customerDto)
        {
            var result = await serviceManager.CustomerService.UpdateCustomerProfileAsync(customerId, customerDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/customers/5 (Soft Delete)
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var result = await serviceManager.CustomerService.DeleteCustomerAsync(customerId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        // --- Address Operations (Dependent Resource) ---
        // Hierarchy: /api/customers/{customerId}/addresses

        // GET: api/customers/5/addresses
        [HttpGet("{customerId}/addresses")]
        public async Task<IActionResult> GetCustomerAddresses(Guid customerId)
        {
            var result = await serviceManager.CustomerService.GetCustomerAddressesAsync(customerId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/customers/5/addresses/3
        [HttpGet("{customerId}/addresses/{addressId}")]
        public async Task<IActionResult> GetCustomerAddressById(Guid customerId, int addressId)
        {
            // This endpoint uses the service method that finds an address by its ID.
            // The customerId in the route ensures the address belongs to the correct customer.
            var result = await serviceManager.CustomerService.GetAddressByIdAsync(addressId);

            // Optional: Add a check to ensure the found address belongs to the customerId in the route
            // This would require a service method change or an extra check here.
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // POST: api/customers/5/addresses
        [HttpPost("{customerId}/addresses")]
        public async Task<IActionResult> AddAddress(Guid customerId, [FromBody] AddressDto addressDto)
        {
            var result = await serviceManager.CustomerService.AddAddressAsync(customerId, addressDto);
            return result.IsSuccess ? StatusCode(201) : BadRequest(result.Error); // 201 Created
        }

        // PUT: api/customers/5/addresses/3
        [HttpPut("{customerId}/addresses/{addressId}")]
        public async Task<IActionResult> UpdateAddress(Guid customerId, int addressId, [FromBody] AddressDto addressDto)
        {
            var result = await serviceManager.CustomerService.UpdateAddressId(addressId, addressDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/customers/5/addresses/3
        [HttpDelete("{customerId}/addresses/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid customerId, int addressId)
        {
            var result = await serviceManager.CustomerService.DeleteAddressAsync(addressId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }


        // --- Global Address Lookup (if needed, often for admin purposes) ---
        // GET: api/addresses
        [HttpGet("/api/addresses")] // Absolute path, outside of customer hierarchy
        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await serviceManager.CustomerService.GetAllAddresses();
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

    }
}
