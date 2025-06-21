using Store.Contracts.AdminContracts.Request.CategoryDTO;
using Store.Contracts.AdminContracts.Response.CategoryDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Abstractions.Services;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> CreateCategory(AdminCreateCategoryDTO categoryDTO)
        {
            var id = Guid.NewGuid();
            var countProductsInCategory = await _categoryRepository.GetCountProductInCategory(id);

            var (category, error) = Category.CreateCategory(id, categoryDTO.Name, categoryDTO.ParentCategoryId,
                countProductsInCategory, new List<Product>(), new List<Category>());

            if (!string.IsNullOrEmpty(error))
                throw new ErrorDuringCreation(error);

            return await _categoryRepository.Create(category);
        }

        public async Task<Guid> DeleteCategory(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _categoryRepository.Delete(id);
        }

        public async Task<IEnumerable<AdminReadCategoryDTO>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAll();
            if (categories == null)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            var dto = categories.Select(MapToDto).ToList();
            return dto;
        }

        public async Task<AdminReadCategoryDTO> GetCategoryById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var category = await _categoryRepository.GetById(id);
            if (category == null)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            var dto = MapToDto(category);

            return dto;
        }

        public async Task<Guid> UpdateCategory(Guid id, AdminUpdateCategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
                throw new BadDataDTO(ErrorMessages.BadDataDTO);
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var category = await _categoryRepository.GetById(id);
            if (category == null)
                throw new NotFound(ErrorMessages.CategoryNotFound);

            category.CategoryName = categoryDTO.Name;
            category.ParentCategoryId = category.ParentCategoryId;

            return await _categoryRepository.Update(id, category);
        }

        public async Task<bool> IsExistsCategory(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _categoryRepository.IsExists(id);
        }

        private AdminReadCategoryDTO MapToDto(Category c)
        {
            return new AdminReadCategoryDTO(
                c.Id,
                c.ParentCategoryId,
                c.CategoryName,
                c.Products.Select(p => new AdminProductInCategoryDTO(
                    p.Id,
                    p.Name
                )).ToList(),

                c.Subcategories.Select(MapToDto).ToList()
            );
        }
    }
}
