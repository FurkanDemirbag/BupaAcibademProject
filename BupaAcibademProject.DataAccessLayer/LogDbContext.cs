using BupaAcibademProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    public class LogDbContext : DbContext
    {
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<EntityLog> EntityLog { get; set; }
        public DbSet<ServiceHistory> ServiceHistory { get; set; }

        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
