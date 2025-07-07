using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.BrandDTO;
using Store.Contracts.AdminContracts.Response.AdminBrandDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;

namespace Store.Application.Services.Admin
{
    public class AdminBrandService : IAdminBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public async Task<Guid> CreateBrand(AdminCreateBrandDTO brandDTO)
        {
            var id = Guid.NewGuid();
            var (brand, error) = Brand.CreateBrand(id, brandDTO.Name, brandDTO.Description, new List<Product>());
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return await _brandRepository.Create(brand);
        }

        public Task<Guid> DeleteBrand(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdminReadBrandDTO>> GetAllBrands()
        {
            throw new NotImplementedException();
        }

        public Task<AdminReadBrandDTO> GetBrandById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UpdateBrand(Guid id, AdminUpdateBrandDTO brandDTO)
        {
            throw new NotImplementedException();
        }
    }
}
