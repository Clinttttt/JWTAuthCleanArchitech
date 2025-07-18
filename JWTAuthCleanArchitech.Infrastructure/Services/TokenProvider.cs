/*using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
    public class TokenProvider
    {
        private readonly ProtectedLocalStorage _storage;

        public TokenProvider(ProtectedLocalStorage storage)
        {
            _storage = storage;
        }

        public async Task<string?> GetTokenAsync()
        {
            var result = await _storage.GetAsync<string>("access_token");
            return result.Success ? result.Value : null;
        }
    }

}
*/
