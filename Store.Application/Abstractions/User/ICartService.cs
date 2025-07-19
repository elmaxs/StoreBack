using Store.Contracts.UserContracts.Request.CartUserDTO;
using Store.Contracts.UserContracts.Response.CartUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface ICartService
    {
        Task<ICollection<ReadCartDTO>> GetCartByUserId(Guid userId);
        Task<Guid> AddCartItem(Guid userId, CartItemDTO dto);
        Task<Guid> ClearCart(Guid userId);
        Task<Guid> UpdateQuantityProductInCart(Guid userId, CartItemDTO dto);
        Task<Guid> DeleteCartItem(Guid userId, Guid productId);
        Task CleanExpiredCartsAsync();
    }
}
