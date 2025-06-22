using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductByCategoryDTO(
        string Name,
        string CategoryName,
        string ImageURL,
        decimal Price,
        int Count
        );
}
