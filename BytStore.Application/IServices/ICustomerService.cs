using ByteStore.Domain.Abstractions;
using BytStore.Application.DTOs.Customer;
using MyResult = ByteStore.Domain.Abstractions.Result;


namespace BytStore.Application.IServices
{
    public interface ICustomerService
    {
        // Customer Profile
        Task<Result<CustomerDto>> GetCustomerProfileByIdAsync(int customerId);
        Task<Result<IEnumerable<CustomerDto>>> GetAllCustomerProfilesAsync();
        Task<MyResult> UpdateCustomerProfileAsync(int customerId, CustomerUpdateDto customerDto);
        Task<MyResult> DeleteCustomerAsync(int customerId);

        // Addresses
        Task<Result<IEnumerable<AddressDto>>> GetCustomerAddressesAsync(int customerId);
        Task<Result<AddressDto>> GetAddressByIdAsync(int addressId);
        Task<Result<IEnumerable<AddressDto>>> GetAllAddresses();
        Task<MyResult> AddAddressAsync(int customerId, AddressDto addressDto);
        Task<MyResult> UpdateAddressId(int addressId, AddressDto addressDto);
        Task<MyResult> DeleteAddressAsync(int addressId);
    }
}
