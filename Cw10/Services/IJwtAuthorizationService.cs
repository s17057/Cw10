using Cw10.DTO.Requests;
using Cw10.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.Services
{
    public interface IJwtAuthorizationService
    {
        public JwtResponse LogIn(LoginRequest request);
        public JwtResponse RefreshToken(String refreshToken);
    }
}
