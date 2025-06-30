using Store.Contracts.UserContracts.Response.CategoryUserDTO;

namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductMainPage(
        CategoriesForMainDTO Category,
        List<ReadProductInByCategoryDTO> Products
        );
}

