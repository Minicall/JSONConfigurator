using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JSONConfigurator.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSONConfigurator
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Configuration>()
                .HasMany(y => y.Children)
                .WithOne(n => n.Parent)
                .HasForeignKey(m => m.ParentId)
                .IsRequired(false);
        }
    }
}
