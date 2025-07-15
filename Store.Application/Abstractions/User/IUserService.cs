using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Abstractions.User
{
    public interface IUserService
    {
        Task Register(string userName, string email, string password);
        Task<string> Login(string email, string password);
    }
}
