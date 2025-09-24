using EmbrujoCerveza.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BeerStyle> BeerStyles => Set<BeerStyle>();
    }
}
