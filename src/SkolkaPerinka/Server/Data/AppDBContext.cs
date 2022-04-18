using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkolkaPerinka.Shared.Models;

namespace SkolkaPerinka.Server.Data
{
    public class AppDBContext : IdentityDbContext
    {
        // pozor pridat dedictvi po  : IdentityDbContext místo DbContext;
        public DbSet<WeatherForecast> Forecasts { get; set; }
        public DbSet<Day> Days { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> option) : base(option)
        {
        }
    }
}
