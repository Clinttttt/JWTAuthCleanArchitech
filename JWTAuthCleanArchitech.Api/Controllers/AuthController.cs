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
using System.Security.Claims;

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
        [Authorize]
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<Movies>> GetAllProduct()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var userid = Guid.Parse(userIdClaim.Value);

            var book = await Bookservice.GetAllMovies(userid);
            if (book is null)
            {
                return BadRequest("No Book here");

            }
            return Ok(book);
        }
        [Authorize]
        [HttpGet("GetBook/{id}")]
        public async Task<ActionResult<Movies>> GetBook(Guid id)
        {
            var UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
           
            var movie = await Bookservice.GetMovies(id);
            return movie is not null && movie.UserId == UserId ? Ok(movie) : BadRequest("idk");

            
        }

        [Authorize]
        [HttpPost("AddBook")]
        public async Task<ActionResult<Movies>> AddBook(MoviesDto moviedto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var userid = Guid.Parse(userIdClaim.Value);
            var moviedtos = moviedto.Adapt<Movies>();
            moviedtos.UserId = userid;
         var book = await Bookservice.AddMovies(moviedtos);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }


        [Authorize]
        [HttpDelete("DeleteMovies/{id}")]
        public async Task<ActionResult<Movies?>> DeleteMovie( Guid id)
        {
            var UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var movies = await Bookservice.GetMovies(id);
            if(movies is null || movies.UserId != UserId)
            {
                return BadRequest("Nothings to delete");
               
            }
           var results =  await Bookservice.DeleteMovies(id);
            return Ok(results);
           
        }
        [Authorize]
        [HttpPut("UpdateMovies")]
        public async Task<ActionResult<Movies>> UpdateMovies(Movies id )
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var Updates = await Bookservice.GetMovies(id.Id);
          

            if (Updates is null || Updates.UserId != userId)
            {
                return BadRequest("something went wrong");
            }
            var updatebook = await Bookservice.UpdateMovies(id);
            return Ok(updatebook);
        }










    }
}


