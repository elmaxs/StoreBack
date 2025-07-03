using Store.Contracts.UserContracts.Response.ProductUserDTO;

namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record CategoriesForMainDTO
    (
        Guid CategoryId,
        string CategoryName,
        List<CategoriesForMainDTO>? Subcategories,
        List<ReadProductForCategoryDTO>? Products // ✅ сюда попадут только к последним подкатегориям
    );
}
