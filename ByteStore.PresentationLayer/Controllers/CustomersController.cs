using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Customer;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    [Authorize]
    public class CustomersController : BaseController
    {
        public CustomersController(IServiceManager serviceManager) : base(serviceManager)
        {
            
        }

        // customer
        // GET: api/customers
        [HttpGet]
        [Authorize(Roles = Roles.AdminRole)]
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


        [HttpGet("user/{customerId}")]
        public async Task<IActionResult> GetCustomerByUserId(string userId)
        {
            var result = await serviceManager.CustomerService.GetCustomerProfileByUserIdAsync(userId);
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
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var result = await serviceManager.CustomerService.DeleteCustomerAsync(customerId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }


        // address
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
            var result = await serviceManager.CustomerService.GetAddressAsync(customerId,addressId);
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
            var result = await serviceManager.CustomerService.UpdateAddressAsync(customerId,addressId, addressDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/customers/5/addresses/3
        [HttpDelete("{customerId}/addresses/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid customerId, int addressId)
        {
            var result = await serviceManager.CustomerService.DeleteAddressAsync(customerId,addressId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }


        // GET: api/addresses
        [HttpGet("/api/addresses")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await serviceManager.CustomerService.GetAllAddresses();
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

    }
}
