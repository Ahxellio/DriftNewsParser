using Microsoft.EntityFrameworkCore;
using DriftNewsParser.Models;

namespace DriftNews.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Championship> Championships { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
