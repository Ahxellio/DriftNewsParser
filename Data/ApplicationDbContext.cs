using Microsoft.EntityFrameworkCore;
using DriftNewsParser.Models;

namespace DriftNews.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ResultsDMEC> ResultsDMEC { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<DriversRDS> DriversRDS { get; set; }
        public DbSet<ResultsRDS> ResultsRDS { get; set; }
        public DbSet<DriversFDPRO> DriversFDPRO { get; set; }
        public DbSet<ResultsFDPRO> ResultsFDPRO { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
