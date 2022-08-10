using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftUserManagement.Application.Features.Commands.AuthenticateUser
{
    public class TokenVM
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
