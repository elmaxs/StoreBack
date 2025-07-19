using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Guid> AddItemAsync(Guid itemId, Guid userId, CartItem item);
        Task<Guid> RemoveItemAsync(Guid userId, Guid productId);
        Task<Guid> UpdateItemQuantityAsync(Guid userId, Guid productId, int newQuantity);
        Task<Guid> ClearCartAsync(Guid userId);
        Task<bool> IsProductInCartAsync(Guid userId, Guid productId);
        Task<List<Cart>?> GetExpiredCartsAsync(DateTime threshold);
        Task SaveChangesAsync();
    }
}
