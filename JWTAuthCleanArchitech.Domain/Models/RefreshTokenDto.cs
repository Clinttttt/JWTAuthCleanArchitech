using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Domain.Models
{
    public class RefreshTokenDto
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
