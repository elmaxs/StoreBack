using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Response.CategoryUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;
using System.Linq;

namespace Store.Application.Services.User
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> CategoryHasProducts(Guid categoryId)
        {
            var countProducts = await _categoryRepository.GetCountProductInCategory(categoryId);

            if (countProducts < 1)
                return false;

            else
                return true;
        }

        private async Task<IEnumerable<ReadMainCategories>> MapSubcategories(IEnumerable<Category> subcategories)
        {
            var subcategoryDTOs = await Task.WhenAll(
                subcategories.Select(async sub =>
                {
                    var nestedSubcategories = sub.Subcategories != null && sub.Subcategories.Any()
                        ? await MapSubcategories(sub.Subcategories)
                        : null;

                    return new ReadMainCategories(
                        sub.Id,
                        sub.CategoryName,
                        sub.Products.Any(),
                        sub.Subcategories != null && sub.Subcategories.Any()
                    );
                })
            );

            return subcategoryDTOs;
        }

        public async Task<IEnumerable<ReadMainCategories>> GetMainsCategories()
        {
            var categories = await _categoryRepository.GetMains();
            if (categories == null || !categories.Any())
                throw new ValidationException(ErrorMessages.CategoryNotFound);

            var categoryDTOs = await Task.WhenAll(
                categories.Select(async c =>
                {
                    var subcategories = c.Subcategories != null && c.Subcategories.Any()
                        ? await MapSubcategories(c.Subcategories)
                        : null;

                    return new ReadMainCategories(
                        c.Id,
                        c.CategoryName,
                        c.Products.Any(),
                        c.Subcategories != null && c.Subcategories.Any()
                    );
                })
            );

            return categoryDTOs;
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
                var mapped = await MappingForGetCategoriesForMainPage(category.Id, 0);
                result.Add(mapped);
            }

            return result;
        }

        private async Task<CategoriesForMainDTO> MappingForGetCategoriesForMainPage(Guid categoryId, int depth = 0)
        {
            var category = await _categoryRepository.GetById(categoryId);
            if (category is null)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            List<CategoriesForMainDTO>? subcategories = null;

            if (depth < 2 && category.Subcategories.Any())
            {
                var firstSub = category.Subcategories.First();
                var mappedSub = await MappingForGetCategoriesForMainPage(firstSub.Id, depth + 1);
                subcategories = new List<CategoriesForMainDTO> { mappedSub };
            }

            return new CategoriesForMainDTO(
                category.Id,
                category.CategoryName
            );
        }


        #region получение мейн категорий мое
        //workalo
        //public async Task<IEnumerable<CategoriesForMainDTO>> GetCategoriesForMainPage()
        //{
        //    var categories = await _categoryRepository.GetMains();
        //    if (categories is null || !categories.Any())
        //        throw new NotFound(ErrorMessages.CategoryNotFound);

        //    var result = new List<CategoriesForMainDTO>();

        //    foreach (var category in categories)
        //    {
        //        var mapped = await MappingForGetCategoriesForMainPage(category.Id);
        //        result.Add(mapped);
        //    }

        //    return result;
        //}

        //workal
        //private async Task<CategoriesForMainDTO> MappingForGetCategoriesForMainPage(Guid categoryId)
        //{
        //    var category = await _categoryRepository.GetById(categoryId);
        //    if (category is null)
        //        throw new NotFound(ErrorMessages.CategoryNotFound);

        //    return new CategoriesForMainDTO(
        //        category.Id,
        //        category.CategoryName,
        //        (await Task.WhenAll(category.Subcategories.Select(async sc => await MappingForGetCategoriesForMainPage(sc.Id))))
        //        .ToList()
        //        //null
        //    );

        //}
        #endregion

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
