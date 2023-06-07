
using DatabaseRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseRepository
{
    public class EfDbContext : DbContext
    {
        public DbSet<RateModel> Rates { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {

            builder.UseSqlServer("Server=DESKTOP-SM098C1;Database=RatesDb;Trusted_Connection=True;TrustServerCertificate=True");
            base.OnConfiguring(builder);
        }
    }
}
