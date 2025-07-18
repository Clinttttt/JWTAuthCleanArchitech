using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Entities;
using JWTAuthCleanArchitech.Infrastructure.Data;
using JWTAuthCleanArchitech.WebUI.Server.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _storage;
        private readonly AuthApiService _authApiService;

        public AuthStateProvider(ProtectedLocalStorage storage, AuthApiService authApiService )
        {
            _storage = storage;
            _authApiService = authApiService;
        }
        

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await _storage.GetAsync<string>("access_token");
                var token = result.Success ? result.Value : null;

                if (string.IsNullOrWhiteSpace(token)) 
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

               
                var claims = new List<Claim>();

                foreach (var claim in jwt.Claims)
                {
            
                    if (claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                    }
                  
                    else if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                    {
                        claims.Add(new Claim(ClaimTypes.Name, claim.Value));
                    }
                
                    else if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    {
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                    }
                    else
                    {
                        claims.Add(new Claim(claim.Type, claim.Value));
                    }
                }

                var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

       
          public async Task LogoutAsync()
           {
               try
               {

                   var tokenResult = await _storage.GetAsync<string>("access_token");
                var token = tokenResult.Success ? tokenResult.Value : null;
                if (!string.IsNullOrWhiteSpace(token))
                {
                     await _authApiService.LogoutAsync(token);
                }


                   await _storage.DeleteAsync("access_token");
                   await _storage.DeleteAsync("refresh_token");

               }
               catch
               {

                   await _storage.DeleteAsync("access_token");
                   await _storage.DeleteAsync("refresh_token");
                   NotifyUserChanged();
               }
           }
   
        public void NotifyUserChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        
       


    }
}