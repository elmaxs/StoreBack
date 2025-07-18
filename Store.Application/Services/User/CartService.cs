using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.CartUserDTO;
using Store.Contracts.UserContracts.Response.CartUserDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services.User
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Guid> AddCartItem(Guid userId, CartItemDTO dto)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var inCart = await _cartRepository.IsProductInCartAsync(userId, dto.ProductId);
            if (inCart)
                return await UpdateQuantityProductInCart(userId, dto);

            var product = await _productRepository.GetById(dto.ProductId);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var (cartItem, error) = CartItem.CreateCartItem(dto.ProductId, product.Name, dto.Quantity, 
                product.Price, product.ImageUrl);

            if (cartItem is not null)
                return await _cartRepository.AddItemAsync(Guid.NewGuid(), userId, cartItem);
            else
                throw new ValidationException(error);
        }

        public async Task<Guid> ClearCart(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            return await _cartRepository.ClearCartAsync(userId);
        }

        public async Task<Guid> DeleteCartItem(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var inCart = await _cartRepository.IsProductInCartAsync(userId, productId);
            if (!inCart)
                throw new ValidationException(ErrorMessages.ProductIsNotInCart);

            return await _cartRepository.RemoveItemAsync(userId, productId);
        }

        public async Task<ICollection<ReadCartDTO>> GetCartByUserId(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var dto = new List<ReadCartDTO>();
            return cart.Items.Select(i => new ReadCartDTO(i.ProductId, i.ProductName, i.ImageUrl, i.Quantity, i.UnitPrice, i.TotalPrice))
                .ToList();
        }

        public async Task<Guid> UpdateQuantityProductInCart(Guid userId, CartItemDTO dto)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            return await _cartRepository.UpdateItemQuantityAsync(userId, dto.ProductId, dto.Quantity);
        }
    }
}
