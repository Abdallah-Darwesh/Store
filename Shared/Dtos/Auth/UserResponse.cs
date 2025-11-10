using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Auth
{
    public  class UserResponse
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }   // ✨ أضفنا IsSuccess
        public string Message { get; set; }

    }
}
