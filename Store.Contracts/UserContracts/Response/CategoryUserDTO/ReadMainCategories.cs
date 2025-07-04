using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contracts.UserContracts.Response.CategoryUserDTO
{
    public record ReadMainCategories(
        Guid Id,
        string CategoryName,
        bool HasProducts,
        bool HasSubcategories
        );
}
