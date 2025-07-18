using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Entities;
using JWTAuthCleanArchitech.Domain.Models;
using JWTAuthCleanArchitech.Infrastructure.Data;
using JWTAuthCleanArchitech.Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;

namespace JWTAuthCleanArchitech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService AuthService, ApplicationDbContext context, IbookService Bookservice) : ControllerBase
    {

        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterAsync(UserDto request)
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
                return BadRequest("invalid username or Password");
            }
            return Ok(results);
        }
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<User>> HandleDelete(UserDto request)
        {
            var results = await AuthService.DeleterUser(request);
            if (results is null)
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

        [Authorize]
        [HttpGet("Check-Admin")]
        public IActionResult CheckAdmin()
        {
            var isAdmin = User.IsInRole("Admin");
            return Ok(new { IsAdmin = isAdmin });
        }
        [Authorize]
        [HttpPost("LogoutUser")]
        public async Task<IActionResult> LogoutAsync()
        {
            var UserIdClaim =  User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (UserIdClaim is null || !Guid.TryParse(UserIdClaim.Value, out var userId)) {
                return BadRequest("Invalid User");

            }
            var FindUser = await AuthService.LogoutUser(userId);
                if(!FindUser)
                {
                    return BadRequest("User Not Found");
                }

            
            return Ok(new { message = "Logout Succesfully" });

        }
        [Authorize]
        [HttpPost("BecomeAdmin")]
        public async Task<ActionResult<TokenResponseDto>> BecomeAdmin()
        {
            var UserIdentity = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (UserIdentity is null || !Guid.TryParse(UserIdentity.Value, out var userIds))
            {
                return BadRequest("User Invalid");
            }
            var token = await AuthService.BecomeAnAdmin(userIds);
                if (token is null)
                {
                    return BadRequest("User Not Found");
                }
          
            return token;



        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<Movies>> GetAllProduct()
        {
            var book = await Bookservice.GetAllMovies();
            if (book is null)
            {
                return BadRequest("No Book here");

            }
            return Ok(book);
        }
        [HttpGet("GetBook/{id}")]
        public async Task<ActionResult<Movies>> GetBook(Guid id)
        {
            var movie = await Bookservice.GetMovies(id);
            return movie is not null ? Ok(movie) : BadRequest("idk");

            
        }

        
        [HttpPost("AddBook")]
        public async Task<ActionResult<Movies>> AddBook(MoviesDto moviedto)
        {
            var moviedtos = moviedto.Adapt<Movies>();
         var book = await Bookservice.AddMovies(moviedtos);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        [HttpDelete("DeleteMovies/{id}")]
        public async Task<ActionResult<Movies?>> DeleteMovie( Guid id)
        {
            var movie = await Bookservice.DeleteMovies(id);
            if(movie is null)
            {
                return BadRequest("Nothings to delete");
               
            }
            return Ok(movie);
           
        }
        [HttpPut("UpdateMovies")]
        public async Task<ActionResult<Movies>> UpdateMovies(Movies id )
        {
            var updatebook = await Bookservice.UpdateMovies(id);
            if (updatebook is null)
            {
                return BadRequest("something went wrong");
            }
      
            return Ok(updatebook);
        }










    }
}


