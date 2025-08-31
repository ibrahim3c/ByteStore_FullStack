using ByteStore.Domain.Abstractions;
using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Customer;
using MyResult = ByteStore.Domain.Abstractions.Result;


namespace BytStore.Application.Services
{
    public class CustomerService
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // Customer Operation
        // get CustomerProfileById
        // get all CustomerProfile
        // update CustomerProfile
        // Delete CustomerProfile => softDelete

        // address operations
        public async Task<Result<IEnumerable<AddressDto>>> GetCustomerAddressesAsync(int customerId)
        {
            var customer = await unitOfWork.CustomerRepository.FindAsync(c => c.Id == customerId, ["Addresses"]);
            if (customer == null)
                return Result<IEnumerable<AddressDto>>.Failure(["No Customer Found."]);
            var addressesDto = customer.Addresses.Select(a => new AddressDto
            {
                City = a.City,
                State = a.State,
                Country = a.Country,
                Id = a.Id,
                IsPrimary = a.IsPrimary,
                PostalCode = a.PostalCode,
                Street = a.Street,
                AddressType = a.AddressType.ToString(),
                CustomerName=customer.fullName
            });
            return  Result<IEnumerable<AddressDto>>.Success(addressesDto);
        }
        public async Task<Result<AddressDto>>GetAddressByIdAsync(int addressId)
        {
            var address = await unitOfWork.GetRepository<Address>().FindAsync(a => a.Id == addressId, ["Customer"]);
            if (address == null)
                return Result<AddressDto>.Failure(["No Address Found."]);
            var addressDto = new AddressDto
            {
                City = address.City,
                State = address.State,
                Country = address.Country,
                Id = address.Id,
                IsPrimary = address.IsPrimary,
                PostalCode = address.PostalCode,
                Street = address.Street,
                AddressType = address.AddressType.ToString(),
                CustomerName=address.Customer.fullName
            };
            
            return Result<AddressDto>.Success(addressDto);
        }
        public async Task<Result<IEnumerable<AddressDto>>> GetAllAddresses()
        {
            var addresses = await unitOfWork.GetRepository<Address>().GetAllAsync(["Customer"]);
            var addressesDto = addresses.Select(a => new AddressDto
            {
                City = a.City,
                State = a.State,
                Country = a.Country,
                Id = a.Id,
                IsPrimary = a.IsPrimary,
                PostalCode = a.PostalCode,
                Street = a.Street,
                AddressType = a.AddressType.ToString(),
                CustomerName=a.Customer.fullName
            });
            return Result<IEnumerable<AddressDto>>.Success(addressesDto);
        }
        public async Task<MyResult> AddAddressAsync(int customerId,AddressDto addressDto)
        {
            // validate dto
            if (!Enum.TryParse<AddressType>(addressDto.AddressType, true, out var addressType))
            {
                return MyResult.Failure(new List<string> { "Invalid address type." });
            }

            var address = new Address
            {
                City = addressDto.City,
                State = addressDto.State,
                Country = addressDto.Country,
                IsPrimary = addressDto.IsPrimary,
                PostalCode = addressDto.PostalCode,
                Street = addressDto.Street,
                CustomerId = customerId,
                AddressType = addressType
            };
            await unitOfWork.GetRepository<Address>().AddAsync(address);
            await unitOfWork.SaveChangesAsync();

            return MyResult.Success();
        }
        // Update Address
        public async Task<MyResult> UpdateAddressId(int addressId, AddressDto addressDto)
        {
            var address = await unitOfWork.GetRepository<Address>().GetByIdAsync(addressId);
            if (address == null)
                return MyResult.Failure(["No Address Found"]);
            
            // validate dto
            if (!Enum.TryParse<AddressType>(addressDto.AddressType, true, out var addressType))
            {
                return MyResult.Failure(new List<string> { "Invalid address type." });
            }
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.Country = addressDto.Country;
            address.IsPrimary = addressDto.IsPrimary;
            address.PostalCode = addressDto.PostalCode;
            address.Street = addressDto.Street;
            address.AddressType = addressType;

            await unitOfWork.GetRepository<Address>().AddAsync(address);
            await unitOfWork.SaveChangesAsync();

            return MyResult.Success();
        }
        public async Task<MyResult>DeleteAddressAsync(int addressId)
        {
         
            var address = await unitOfWork.GetRepository<Address>().GetByIdAsync(addressId);
            if (address == null)
                return MyResult.Failure(["No Address Found"]);

            unitOfWork.GetRepository<Address>().Delete(address);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }

    }
}
