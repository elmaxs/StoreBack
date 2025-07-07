using Store.Contracts.AdminContracts.Request.BrandDTO;
using Store.Contracts.AdminContracts.Response.AdminBrandDTO;

namespace Store.Application.Abstractions.Admin
{
    public interface IAdminBrandService
    {
        Task<Guid> CreateBrand(AdminCreateBrandDTO brandDTO);
        Task<IEnumerable<AdminReadBrandDTO>> GetAllBrands();
        Task<AdminReadBrandDTO> GetBrandById(Guid id);
        Task<Guid> UpdateBrand(Guid id, AdminUpdateBrandDTO brandDTO);
        Task<Guid> DeleteBrand(Guid id);
    }
}
