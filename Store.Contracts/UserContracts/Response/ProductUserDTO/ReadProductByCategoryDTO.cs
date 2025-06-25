using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductByCategoryDTO(
        Guid CategoryId,
        string? CategoryName,
        List<ReadProductInByCategoryDTO>? Products
        );

    public record ReadProductInByCategoryDTO(
        Guid? ProductId,
        string? ProductName,
        string? ImageURL,
        decimal? Price
        );
}
