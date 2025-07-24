using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Models;
using JWTAuthCleanArchitech.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
  public  class BookService(ApplicationDbContext context) : IbookService
    {

    public async Task<IEnumerable<Movies?>> GetAllMovies(Guid userId)
        {




            return await context.movies.Where(u=> u.UserId == userId).
                ToListAsync();
        }
    public async Task<Movies?> AddMovies(Movies movie)
        { 
         var movies = context.movies.Add(movie).Entity;
            await context.SaveChangesAsync();
            return movies;
        }
    public async Task<Movies?> UpdateMovies (Movies movie)
        {
            var moviess = await context.movies.FindAsync(movie.Id);
            if (moviess is null)
            {
                return null;
            }
            moviess.MovieTitle = movie.MovieTitle;
            moviess.Description = movie.Description;
            moviess.Genre = movie.Genre;

            context.movies.Update(moviess);
           await context.SaveChangesAsync();

            return moviess;
        }
      

        public async Task<Movies?> GetMovies (Guid id)
        {
    
            var movies = await context.movies.FirstOrDefaultAsync(u => u.Id == id);
            
            return movies;
          
        }
        public async Task<Movies?> DeleteMovies (Guid id)
        {
            var deletemovie = await context.movies.FirstOrDefaultAsync(u => u.Id == id);
            if( deletemovie is null)
            {
                return null;
            }
            context.movies.Remove(deletemovie);
            await context.SaveChangesAsync();
            return deletemovie;
        }
     
    }
}
