using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.BrandDTO;
using Store.Contracts.AdminContracts.Response.AdminBrandDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services.Admin
{
    public class AdminBrandService : IAdminBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public AdminBrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Guid> CreateBrand(AdminCreateBrandDTO brandDTO)
        {
            var id = Guid.NewGuid();
            var (brand, error) = Brand.CreateBrand(id, brandDTO.Name, brandDTO.Description, new List<Product>());
            if (!string.IsNullOrEmpty(error) || brand is null)
                throw new ErrorDuringCreation(error);

            return await _brandRepository.Create(brand);
        }

        public async Task<Guid> DeleteBrand(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _brandRepository.Delete(id);
        }

        public async Task<IEnumerable<AdminReadBrandDTO>> GetAllBrands()
        {
            var brands = await _brandRepository.GetAll();
            if (brands is null || !brands.Any())
                throw new NotFound(ErrorMessages.BrandNotFound);

            var brandsDTO = brands.Select(b => new AdminReadBrandDTO(b.Id, b.Name, b.Description)).ToList();

            return brandsDTO;
        }

        public async Task<AdminReadBrandDTO> GetBrandById(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var brand = await _brandRepository.GetById(id);
            if (brand is null)
                throw new NotFound(ErrorMessages.BrandNotFound);

            var brandDTO = new AdminReadBrandDTO(brand.Id, brand.Name, brand.Description);

            return brandDTO;
        }

        public async Task<Guid> UpdateBrand(Guid id, AdminUpdateBrandDTO brandDTO)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var brand = await _brandRepository.GetById(id);
            if (brand is null)
                throw new NotFound(ErrorMessages.BrandNotFound);

            brand.Name = brandDTO.Name;
            brand.Description = brandDTO.Description;

            return await _brandRepository.Update(id, brand);
        }
    }
}
