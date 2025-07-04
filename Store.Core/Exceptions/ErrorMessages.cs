namespace Store.Core.Exceptions
{
    public static class ErrorMessages
    {
        public const string GuidCannotBeEmpty = "Id cant be empty Guid!";
        public const string CategoryNotFound = "Category not found";
        public const string ProductNotFound = "Product not found";
        public const string OrderNotFound = "Order not found";
        public const string OrderItemNotFound = "Order item not found";
        public const string UserNotFound = "User not found";
        public const string BadDataDTO = "DTO cant be null";
        public const string ModelCantBeCreate = "Model cant be create";
        public const string CategoryNotHasProducts = "There are no products in the category";
    }
}
