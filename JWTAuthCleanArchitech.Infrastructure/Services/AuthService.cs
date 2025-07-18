using Azure.Core;
using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Entities;
using JWTAuthCleanArchitech.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
    class AuthService(ApplicationDbContext context, IConfiguration configuration) : IAuthService
    {

       
        public async Task<TokenResponseDto?> HandleLogin(UserDto request)
        {
            var user = await context.users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return await CreateTokenResponse(user);
        }
        public async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await context.users.AnyAsync(u => u.UserName == request.UserName))
            {
                return null;
            }
            var user = new User();
            var hashPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.UserName = request.UserName;
            user.PasswordHash = hashPassword;

            context.users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        private string GenerateRefreshToken()
        {
            var RandomNumber = new Byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(RandomNumber);
            return Convert.ToBase64String(RandomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var RefreshToken = GenerateRefreshToken();
            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return RefreshToken;
        }
        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenDto request)
        {
            var user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null)

                return null;

            return await CreateTokenResponse(user);
        }
        private async Task<User?> ValidateRefreshToken(Guid UserId, string RefreshToken)
        {
            var user = await context.users.FindAsync(UserId);
            if (user is null || user.RefreshToken != RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
               new Claim(ClaimTypes.Role, user.Role)

            };


            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var TokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds

                );
            return new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
        }

        public async Task<User?> DeleterUser(UserDto request)
        {
            var user = await context.users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user is null)
            {
                return null;
            }
            context.users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> LogoutUser(Guid id )
        {
            var user = await context.users.FindAsync(id);
            if(user is null)
            {
                return false;
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<TokenResponseDto?> BecomeAnAdmin(Guid id)
        {
            var user = await context.users.FindAsync(id);
            if(user is null)
            {
                return null;
            }
            user.Role = "Admin";
            
            await context.SaveChangesAsync();
            return await CreateTokenResponse(user);
        }


    }
}

