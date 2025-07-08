using System.ComponentModel.DataAnnotations;

namespace Store.Contracts.AdminContracts.Request.BrandDTO
{
    public record AdminCreateBrandDTO(
        [Required] string Name,
        [Required] string Description
        );
}
