using AirplaneApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirplaneApi.Data
{
    public class AirplaneContext : DbContext
    {
        public AirplaneContext(DbContextOptions<AirplaneContext> options) : base(options)
        {
        }
        public AirplaneContext()
        {
        }

        public DbSet<Airplane> Airplanes{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airplane>().ToTable("Airplane");
        }
    }
}
