using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Services
{
   public interface IbookService
    {
        Task<Movies?> AddMovies(Movies movie);
        Task<IEnumerable<Movies?>> GetAllMovies(Guid userId);
      
        Task<Movies?> UpdateMovies(Movies movie);
     
        Task<Movies?> GetMovies(Guid id);
        Task<Movies?> DeleteMovies(Guid id);
   
    }
}
