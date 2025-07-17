using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contracts.UserContracts.Request.ProductReviewDTO
{
    public record CreateProductReviewDTO(
        Guid UserId,
        Guid ProductId,
        string Text,
        int? Rating);
}
