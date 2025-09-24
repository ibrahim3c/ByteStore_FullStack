using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Customer;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IUnitOfWork unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // Customer Operation
        public async Task<Result2<CustomerDto>> GetCustomerProfileByIdAsync(Guid customerId)
        { 
            var customer=await unitOfWork.CustomerRepository.FindAsync(c => c.Id == customerId, ["AppUser"]);
            if (customer == null)
                return Result2<CustomerDto>.Failure(CustomerErrors.NotFound);
            if(customer.AppUser ==null)
                return Result2<CustomerDto>.Failure(UserErrors.NotFound);
            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                DateOfBirth = customer.DateOfBirth,
                Email = customer.AppUser.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.AppUser.PhoneNumber
            };
            return Result2<CustomerDto>.Success(customerDto);
        }
        public async Task<Result2<IEnumerable<CustomerDto>>> GetAllCustomerProfilesAsync()
        {
            var customers = await unitOfWork.CustomerRepository.GetAllAsync(["AppUser"]);

            var customerDtos = customers.Select(customer => new CustomerDto
            {
                Id=customer.Id,
                DateOfBirth = customer.DateOfBirth,
                Email = customer.AppUser?.Email, // safe navigation
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.AppUser?.PhoneNumber
            }).ToList();

            return Result2<IEnumerable<CustomerDto>>.Success(customerDtos);
        }
        public async Task<Result2> UpdateCustomerProfileAsync(Guid customerId, CustomerUpdateDto customerDto)
        {
            var customer = await unitOfWork.CustomerRepository.FindAsync(c => c.Id == customerId, ["AppUser"]);
            if (customer == null)
                return Result2.Failure(CustomerErrors.NotFound);
            if (customer.AppUser == null)
                return Result2.Failure(UserErrors.NotFound);

            customer.DateOfBirth = customerDto.DateOfBirth;
            customer.FirstName=customerDto.FirstName;
            customer.LastName=customerDto.LastName;
            customer.AppUser.PhoneNumber = customerDto.PhoneNumber;

            unitOfWork.CustomerRepository.Update(customer);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        // Delete CustomerProfile => softDelete
        public async Task<Result2> DeleteCustomerAsync(Guid customerId)
        {
            var customer = await unitOfWork.CustomerRepository.FindAsync(c=>c.Id==customerId);
            if (customer == null)
                return Result2.Failure(CustomerErrors.NotFound);

            customer.IsDeleted = true;

            unitOfWork.CustomerRepository.Update(customer);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }


        // address operations
        public async Task<Result2<IEnumerable<AddressDto>>> GetCustomerAddressesAsync(Guid customerId)
        {
            var customer = await unitOfWork.CustomerRepository.FindAsync(c => c.Id == customerId, ["Addresses"]);
            if (customer == null)
                return Result2<IEnumerable<AddressDto>>.Failure(CustomerErrors.NotFound);
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
            return Result2<IEnumerable<AddressDto>>.Success(addressesDto);
        }
        public async Task<Result2<AddressDto>>GetAddressAsync(Guid customerId,int addressId)
        {
            var address = await unitOfWork.GetRepository<Address>().FindAsync(a => a.Id == addressId && a.CustomerId == customerId, ["Customer"]);

            if (address == null)
                return Result2<AddressDto>.Failure(AddressErrors.NotFound);
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
            
            return Result2<AddressDto>.Success(addressDto);
        }
        public async Task<Result2<IEnumerable<AddressDto>>> GetAllAddresses()
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
            return Result2<IEnumerable<AddressDto>>.Success(addressesDto);
        }
        public async Task<Result2> AddAddressAsync(Guid customerId,AddressDto addressDto)
        {
            // validate dto
            if (!Enum.TryParse<AddressType>(addressDto.AddressType, true, out var addressType))
            {
                return Result2.Failure(AddressErrors.InvalidType);
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

            return Result2.Success();
        }
        public async Task<Result2> UpdateAddressAsync(Guid customerId, int addressId, AddressDto addressDto)
        {
            var address = await unitOfWork.GetRepository<Address>().FindAsync(a => a.Id == addressId && a.CustomerId == customerId);
            if (address == null)
                return Result2.Failure(AddressErrors.NotFound);
            
            // validate dto
            if (!Enum.TryParse<AddressType>(addressDto.AddressType, true, out var addressType))
            {
                return Result2.Failure(AddressErrors.InvalidType);
            }
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.Country = addressDto.Country;
            address.IsPrimary = addressDto.IsPrimary;
            address.PostalCode = addressDto.PostalCode;
            address.Street = addressDto.Street;
            address.AddressType = addressType;

            unitOfWork.GetRepository<Address>().Update(address);
            await unitOfWork.SaveChangesAsync();

            return Result2.Success();
        }
        public async Task<Result2> DeleteAddressAsync(Guid customerId,int addressId)
        {
            var address = await unitOfWork.GetRepository<Address>().FindAsync(a => a.Id == addressId && a.CustomerId == customerId);
            if (address == null)
                return Result2.Failure(AddressErrors.NotFound);

            unitOfWork.GetRepository<Address>().Delete(address);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }

    }
}
