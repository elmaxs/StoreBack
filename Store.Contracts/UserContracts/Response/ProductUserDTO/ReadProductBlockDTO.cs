using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contracts.UserContracts.Response.ProductUserDTO
{
    public record ReadProductBlockDTO(
        Guid CategoryId,
        string CategoryName,
        List<ReadProductDTO> Products
        );
}
