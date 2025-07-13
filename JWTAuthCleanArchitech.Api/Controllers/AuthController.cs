using JWTAuthCleanArchitech.Domain.Entities;
using JWTAuthCleanArchitech.Domain.Models;
using JWTAuthCleanArchitech.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace JWTAuthCleanArchitech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService AuthService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<User?>> RegisterAsync(UserDto request)
        {
            var user = await AuthService.RegisterAsync(request);
            if (user is null)
            {
               return BadRequest("User is already exists");
            
            }
            return Ok(user);
        
          
        }
        [HttpPost("Login")]
        public async Task<ActionResult<User?>> HandleLogin(UserDto request)
        {
            var results = await AuthService.HandleLogin(request);
            if (results is null)
            {
               return  BadRequest("invalid username or Password");
            }
            return Ok(results);
        }
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<User>> HandleDelete(UserDto request)
        {
            var results = await AuthService.DeleterUser(request);
            if(results is null)
            {
                return BadRequest("User not Exists");
            }
            return Ok(results);
        }




        [HttpPost("Request-Token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenDto request)
        {
            var result = await AuthService.RefreshTokenAsync(request);
            if (result is null || request.RefreshToken is null || result.RefreshToken is null)

                return Unauthorized("Invalid Refresh Token");

            return Ok(result);
        }


        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndPoints()
        {
            return Ok("You are Authenticated");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin-Only")]
        public IActionResult AdminOnlyEndPoints()
        {
            return Ok("You are an Admin!");
        }


    }
}

