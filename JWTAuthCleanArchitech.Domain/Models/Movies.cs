using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Domain.Models
{
   public  class Movies
    {
        public Guid Id { get; set; }
       public string? MovieTitle { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public Guid UserId { get; set; }
    }
}
