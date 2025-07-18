using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly OnlineStoreDbContext _context;

        public CartRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddItemAsync(Guid itemId, Guid userId, CartItem item)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var cartItem = new CartItemEntity
            {
                Id = itemId,
                CartId = userId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                AddedAt = DateTime.UtcNow
            };

            cart.Items.Add(cartItem);

            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<Guid> ClearCartAsync(Guid userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            cart?.Items.Clear();

            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            var cartEntity = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartEntity is null)
                return null;

            var cart = Cart.CreateCart(cartEntity.UserId, cartEntity.LastUpdated, cartEntity.Items.Select(i =>
            CartItem.CreateCartItem(i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice, i.Product.ImageUrl).CartItem)
                .ToList()).Cart;

            return cart;
        }

        public async Task<bool> IsProductInCartAsync(Guid userId, Guid productId)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            return cart.Items.Any(p => p.ProductId == productId);
        }

        public async Task<Guid> RemoveItemAsync(Guid userId, Guid productId)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var cartItem = cart.Items.FirstOrDefault(p => p.ProductId == productId);
            if (cartItem is null)
                throw new NotFound(ErrorMessages.ProductIsNotInCart);

            cart.Items.Remove(cartItem);

            await _context.CartItems.Where(i => i.Id == cartItem.Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<Guid> UpdateItemQuantityAsync(Guid userId, Guid productId, int newQuantity)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
               .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if(cartItem is null)
                throw new NotFound(ErrorMessages.ProductIsNotInCart);

            cartItem.Quantity = newQuantity;

            await _context.SaveChangesAsync();

            return userId;
        }
    }
}
