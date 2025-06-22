using Store.Contracts.UserContracts.Response.ProductUserDTO;
using Store.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Abstractions.User
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>>? GetProductsByCategoryId(Guid categoryId);
    }
}
