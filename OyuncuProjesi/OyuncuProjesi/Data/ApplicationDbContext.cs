using Microsoft.EntityFrameworkCore;
using OyuncuProjesi.Models;

namespace OyuncuProjesi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Gamer> Gamers { get; set; }
    }
}
