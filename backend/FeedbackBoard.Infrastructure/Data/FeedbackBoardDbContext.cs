using Microsoft.EntityFrameworkCore;
using FeedbackBoard.Domain.Entities;
using System.Reflection;

namespace FeedbackBoard.Infrastructure.Data
{
    /// <summary>
    /// Database context for the Feedback Board application
    /// </summary>
    public class FeedbackBoardDbContext : DbContext
    {
        public FeedbackBoardDbContext(DbContextOptions<FeedbackBoardDbContext> options) : base(options)
        {
        }

        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configure Feedback entity
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                
                // Add index for common queries
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.Rating);
            });
        }
    }
}
