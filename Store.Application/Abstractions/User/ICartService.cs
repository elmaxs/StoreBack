using Store.Contracts.UserContracts.Request.CartUserDTO;
using Store.Contracts.UserContracts.Response.CartUserDTO;

namespace Store.Application.Abstractions.User
{
    public interface ICartService
    {
        Task<ReadCartDTO> GetCartByUserId(Guid userId);
        Task<int> AddCartItem(Guid userId, CartItemDTO dto);
        Task<Guid> ClearCart(Guid userId);
        Task<int> UpdateQuantityProductInCart(Guid userId, CartItemDTO dto);
        Task<Guid> DeleteCartItem(Guid userId, Guid productId);
        Task CleanExpiredCartsAsync();
        Task<ICollection<ProductIdsInCartDTO>> GetProductIdsInCarts(Guid userId);
    }
}
