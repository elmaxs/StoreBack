using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Response.CategoryUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services.User
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ReadCategoriesDTO>> GetMainsCategories()
        {
            var categories = await _categoryRepository.GetMains();
            if (categories is null)
                throw new ValidationException(ErrorMessages.CategoryNotFound);

            var categoryDTO = categories.Select(c => new ReadCategoriesDTO(c.Id, c.CategoryName)).ToList();

            return categoryDTO;
        }

        public async Task<IEnumerable<BreadcrumbCategoryDTO>> BuildBreadcrumbAsync(Guid currentCategoryId)
        {
            var breadcrumbs = new List<BreadcrumbCategoryDTO>();

            if (currentCategoryId == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            await MappingBreadcrumbsCategories(currentCategoryId, breadcrumbs);

            if (breadcrumbs.Count == 0)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            breadcrumbs.Reverse();

            return breadcrumbs;
        }

        public async Task<IEnumerable<(Guid, string)>> GetAllNestedCategoryIdsAndNames(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new InvalidOperationException(ErrorMessages.GuidCannotBeEmpty);

            List<(Guid, string)> hierarchy = new List<(Guid, string)>();

            await RecursiveHierarchy(categoryId, null, hierarchy);
            if (hierarchy.Count == 0)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            return hierarchy;
        }

        public async Task<IEnumerable<CategoriesForMainDTO>> GetCategoriesForMainPage()
        {
            var categories = await _categoryRepository.GetMains();
            if (categories is null || !categories.Any())
                throw new NotFound(ErrorMessages.CategoryNotFound);

            var result = new List<CategoriesForMainDTO>();

            foreach (var category in categories)
            {
                var mapped = await MappingForGetCategoriesForMainPage(category.Id);
                result.Add(mapped);
            }

            return result;
        }


        //private async Task<CategoriesForMainDTO> MappingForGetCategoriesForMainPage(Guid categoryId)
        //{
        //    var category = await _categoryRepository.GetById(categoryId);
        //    if (category is null)
        //        throw new InvalidOperationException(ErrorMessages.CategoryNotFound);

        //    // Мапим подкатегории рекурсивно
        //    var subcategoryDTOs = await Task.WhenAll(
        //        category.Subcategories.Select(async sc =>
        //            await MappingForGetCategoriesForMainPage(sc.Id))
        //    );

        //    // Возвращаем собранный DTO
        //    return new CategoriesForMainDTO(
        //        category.Id,
        //        category.CategoryName,
        //        subcategoryDTOs.ToList()
        //    );
        //}

        private async Task<CategoriesForMainDTO> MappingForGetCategoriesForMainPage(Guid categoryId)
        {
            var category = await _categoryRepository.GetById(categoryId);
            if (category is null)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            return new CategoriesForMainDTO(
                category.Id,
                category.CategoryName,
                (await Task.WhenAll(category.Subcategories.Select(async sc => await MappingForGetCategoriesForMainPage(sc.Id))))
                .ToList(),
                null
            );

        }

        private async Task RecursiveHierarchy(Guid categoryId, string? categoryName, List<(Guid, string)> hierarchy)
        {
            var category = await _categoryRepository.GetById(categoryId);
            if (category is null)
                return;

            hierarchy.Add((categoryId, category.CategoryName));

            if (!category.Products.Any())
            {
                foreach (var sub in category.Subcategories)
                    await RecursiveHierarchy(sub.Id, sub.CategoryName, hierarchy);
            }
        }

        //дерево мб позже
        //public async Task<TreeCategoryDTO> BuildTreeCategory(Guid currentCategoryId)
        //{
        //    if (currentCategoryId == Guid.Empty)
        //        throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);


        //}

        //private async Task<TreeCategoryDTO> MappingTreeCategories(Guid categoryId)
        //{
        //    var category = await _categoryRepository.GetById(categoryId);
        //    if (category is null)
        //        return null;

        //    var categoryDTO = new TreeCategoryDTO(category.Id, category.CategoryName, )
        //}

        private async Task MappingBreadcrumbsCategories(Guid categoryId, List<BreadcrumbCategoryDTO> breadcrumbs)
        {
            var category = await _categoryRepository.GetById(categoryId);
            if (category is null)
                return;
            
            breadcrumbs.Add(new BreadcrumbCategoryDTO(category.Id, category.CategoryName));

            if (category.ParentCategoryId.HasValue)
                await MappingBreadcrumbsCategories(category.ParentCategoryId.Value, breadcrumbs);
        }
    }
}
