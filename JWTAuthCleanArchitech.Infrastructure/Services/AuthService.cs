using JWTAuthCleanArchitech.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
    class AuthService(ApplicationDbContext _db, IConfiguration configuration) : IAuthService
    {
       
       
    }
}
