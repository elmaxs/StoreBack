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

        public async Task CleanExpiredCartsAsync()
        {
            var expirationTime = DateTime.UtcNow.AddMinutes(-30);
            var expiredCarts = await _cartRepository.GetExpiredCartsAsync(expirationTime);

            if (expiredCarts is null)
                return;

            foreach(var cart in expiredCarts)
            {
                foreach(var item in cart.Items)
                {
                    var product = await _productRepository.GetById(item.ProductId);
                    if (product is null)
                        throw new NotFound(ErrorMessages.ProductNotFound);

                    product.ReservedQuantity = Math.Max(0, product.ReservedQuantity - item.Quantity);
                    await _productRepository.Update(product.Id, product);
                    Console.WriteLine("!!!!!!!!!!!!!!WAS CLEAR!!!!!!!!!!!!!!!!!!!!!!");
                }
                await ClearCart(cart.UserId);
                cart.LastUpdated = DateTime.UtcNow;
            }
            await _cartRepository.SaveChangesAsync();
        }

        private async Task<int> GetAvailableQuantity(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var isAvailable = product.IsAvailable && product.AvailableQuantity >= quantity;

            return isAvailable ? quantity : product.AvailableQuantity;
        }

        public async Task<ICollection<ProductIdsInCartDTO>> GetProductIdsInCarts(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            return cart.Items.Select(i => new ProductIdsInCartDTO(i.ProductId)).ToList();
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

            var availableQuantity = await GetAvailableQuantity(dto.ProductId, dto.Quantity);

            var (cartItem, cartItemError) = CartItem.CreateCartItem(dto.ProductId, product.Name, availableQuantity, 
                product.Price, product.ImageUrl);

            if (!string.IsNullOrEmpty(cartItemError))
                throw new ValidationException(cartItemError);

            var (updateProduct, updateProductError) = Product.CreateProduct(product.Id, product.Name, product.Description, product.ImageUrl,
                product.Price, product.CategoryId, product.CategoryName, product.BrandId, product.BrandName,
                product.StockQuantity, product.ReservedQuantity += availableQuantity);

            if (!string.IsNullOrEmpty(updateProductError))
                throw new ValidationException(updateProductError);

            await _productRepository.Update(product.Id, updateProduct);

            return await _cartRepository.AddItemAsync(Guid.NewGuid(), userId, cartItem);
        }

        public async Task<Guid> ClearCart(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetById(item.ProductId);
                if (product is null)
                    throw new NotFound(ErrorMessages.ProductNotFound);

                product.ReservedQuantity = Math.Max(0, product.ReservedQuantity - item.Quantity);
                await _productRepository.Update(product.Id, product);
                Console.WriteLine("!!!!!!!!!!!!!!WAS CLEAR!!!!!!!!!!!!!!!!!!!!!!");
            }
            cart.LastUpdated = DateTime.UtcNow;

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

            var product = await _productRepository.GetById(productId);
            if (product is null)
                throw new NotFound(ErrorMessages.ProductNotFound);

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            product.ReservedQuantity = Math.Max(0, product.ReservedQuantity - item.Quantity);
            await _productRepository.Update(product.Id, product);

            return await _cartRepository.RemoveItemAsync(userId, productId);
        }

        public async Task<ReadCartDTO> GetCartByUserId(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var totalPrice = cart.Items.Sum(i => i.TotalPrice);
            var itemCart = cart.Items.Select(i => new ItemCartDTO(i.ProductId, i.ProductName, i.ImageUrl, i.Quantity, i.UnitPrice, 
                i.TotalPrice)).ToList();

            return new ReadCartDTO(totalPrice, itemCart);
        }

        public async Task<Guid> UpdateQuantityProductInCart(Guid userId, CartItemDTO dto)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                throw new NotFound(ErrorMessages.CartNotFound);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (cartItem is null)
                throw new NotFound(ErrorMessages.ProductIsNotInCart);

            if(cartItem.Quantity < dto.Quantity)
            {
                var availableQuantity = await GetAvailableQuantity(dto.ProductId, dto.Quantity-cartItem.Quantity);
                if (availableQuantity <= 0)
                    throw new ValidationException("No available quantity");

                var product = await _productRepository.GetById(dto.ProductId);

                var (updateProduct, error) = Product.CreateProduct(product.Id, product.Name, product.Description, product.ImageUrl,
                    product.Price, product.CategoryId, product.CategoryName, product.BrandId, product.BrandName,
                    product.StockQuantity, product.ReservedQuantity += availableQuantity);

                if (!string.IsNullOrEmpty(error))
                    throw new ValidationException(error);

                await _productRepository.Update(product.Id, updateProduct);

                return await _cartRepository.UpdateItemQuantityAsync(userId, dto.ProductId, cartItem.Quantity + availableQuantity);
            }
            else
            {
                var product = await _productRepository.GetById(dto.ProductId);

                var (updateProduct, error) = Product.CreateProduct(product.Id, product.Name, product.Description, product.ImageUrl,
                    product.Price, product.CategoryId, product.CategoryName, product.BrandId, product.BrandName,
                    product.StockQuantity, product.ReservedQuantity -= cartItem.Quantity - dto.Quantity);

                if (!string.IsNullOrEmpty(error))
                    throw new ValidationException(error);

                await _productRepository.Update(product.Id, updateProduct);

                return await _cartRepository.UpdateItemQuantityAsync(userId, dto.ProductId, dto.Quantity);
            }
            //    var availableQuantity = await GetAvailableQuantity(dto.ProductId, dto.Quantity);
            //if (availableQuantity <= 0)
            //    throw new ValidationException("No available quantity");

            //var product = await _productRepository.GetById(dto.ProductId);

            //var (updateProduct, error) = Product.CreateProduct(product.Id, product.Name, product.Description, product.ImageUrl,
            //    product.Price, product.CategoryId, product.CategoryName, product.BrandId, product.BrandName,
            //    product.StockQuantity, product.ReservedQuantity += availableQuantity);

            //if (!string.IsNullOrEmpty(error))
            //    throw new ValidationException(error);

            //await _productRepository.Update(product.Id, updateProduct);

            //return await _cartRepository.UpdateItemQuantityAsync(userId, dto.ProductId, cartItem.Quantity + availableQuantity);
        }
    }
}
