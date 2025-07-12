using JWTAuthCleanArchitech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure.Data
{
   public class ApplicationDbContext : DbContext
    {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {       
        }
        public DbSet<User> users { get; set; }
        
    }

      
}
