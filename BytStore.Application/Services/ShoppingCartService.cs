using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.ShoppingCart;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    public class ShoppingCartService:IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }
        public async Task<Result2> ClearCartAsync(string id)
        {
          var isSuccess= await shoppingCartRepository.ClearCartAsync(id);
            if (!isSuccess)
                return Result2.Failure(CartErrors.ClearFailed);
            return Result2.Success();
        }
        public async Task<Result2<ShoppingCartDto>> GetCartAsync(string id)
        {
           var cart=await shoppingCartRepository.GetCartAsync(id);
           if(cart == null)
                return  Result2<ShoppingCartDto>.Failure(CartErrors.NotFound);
            var cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(c => new CartItemDto
                {
                    BrandName = c.BrandName,
                    CategoryName = c.CategoryName,
                    ImageUrl = c.ImageUrl,
                    Name = c.Name,
                    Price = c.Price,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList()
            };

           return Result2<ShoppingCartDto>.Success(cartDto);
        }

        // for add or update
        public async Task<Result2> SaveCartAsync(ShoppingCartDto cartDto)
        {
            var cart = new ShoppingCart
            {
                Id = cartDto.Id,
                CartItems = cartDto.CartItems.Select(c => new CartItem
                {
                    BrandName = c.BrandName,
                    CategoryName = c.CategoryName,
                    ImageUrl = c.ImageUrl,
                    Name = c.Name,
                    Price = c.Price,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList()
            };
            var isSuccess = await shoppingCartRepository.SaveCartAsync(cart);
            if (!isSuccess)
                return Result2.Failure(CartErrors.SaveFailed);
            return Result2.Success();
        }
    }
}
