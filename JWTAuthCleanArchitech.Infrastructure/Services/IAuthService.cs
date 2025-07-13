using JWTAuthCleanArchitech.Domain.Entities;
using JWTAuthCleanArchitech.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
  public  interface IAuthService
    {
       Task<User?> RegisterAsync(UserDto request);
       Task<TokenResponseDto?> HandleLogin(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenDto request);
        Task<User?> DeleterUser(UserDto request);
    }
}
