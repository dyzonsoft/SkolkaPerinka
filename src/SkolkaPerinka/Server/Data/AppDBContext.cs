﻿using Microsoft.EntityFrameworkCore;
using SkolkaPerinka.Shared.Models;

namespace SkolkaPerinka.Server.Data
{
    public class AppDBContext : DbContext
    {
        // pozor pridat dedictvi po  : IdentityDbContext místo DbContext;
        //public DbSet<WeatherForecast> Forecast { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> option) : base(option)
        {
        }
    }
}