using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    internal class ShoppingCartService:IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }
        public async Task<Result2> ClearCartAsync(string customerId)
        {
          var isSuccess= await shoppingCartRepository.ClearCartAsync(customerId);
            if (!isSuccess)
                return Result2.Failure(CartErrors.ClearFailed);
            return Result2.Success();
        }
        public async Task<Result2<ShoppingCart>> GetCartAsync(string customerId)
        {
           var cart=await shoppingCartRepository.GetCartAsync(customerId);
           if(cart == null)
                return  Result2<ShoppingCart>.Failure(CartErrors.NotFound);

           return Result2<ShoppingCart>.Success(cart);
        }

        // for add or update
        public async Task<Result2> SaveCartAsync(ShoppingCart cart)
        {
            var isSuccess = await shoppingCartRepository.SaveCartAsync(cart);
            if (!isSuccess)
                return Result2.Failure(CartErrors.SaveFailed);
            return Result2.Success();
        }
    }
}
