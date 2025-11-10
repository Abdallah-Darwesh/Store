using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.Auth;

namespace Store.Services.Abstractions.Auth
{
    public interface IAuthService
    {
       Task<UserResponse> LogicAsync(LoginRequest request);
       Task<UserResponse> RegisterAsync(RegisterRequest request);

    }
}
