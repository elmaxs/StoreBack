using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Response.CategoryUserDTO;
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
