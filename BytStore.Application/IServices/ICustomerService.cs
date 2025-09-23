using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Customer;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;


namespace BytStore.Application.IServices
{
    public interface ICustomerService
    {
        // Customer Profile
        Task<Result2<CustomerDto>> GetCustomerProfileByIdAsync(Guid customerId);
        Task<Result2<IEnumerable<CustomerDto>>> GetAllCustomerProfilesAsync();
        Task<Result2> UpdateCustomerProfileAsync(Guid customerId, CustomerUpdateDto customerDto);
        Task<Result2> DeleteCustomerAsync(Guid customerId);

        // Addresses
        Task<Result2<IEnumerable<AddressDto>>> GetCustomerAddressesAsync(Guid customerId);
        Task<Result2<AddressDto>> GetAddressAsync(Guid customerId, int addressId);
        Task<Result2<IEnumerable<AddressDto>>> GetAllAddresses();
        Task<Result2> AddAddressAsync(Guid customerId, AddressDto addressDto);
        Task<Result2> UpdateAddressAsync(Guid customerId, int addressId, AddressDto addressDto);
        Task<Result2> DeleteAddressAsync(Guid customerId, int addressId);
    }
}
