using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSets for your models
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationship between Booking and Resource
            modelBuilder.Entity<Booking>()
      .HasOne(b => b.Resource)
      .WithMany(r => r.Bookings)
      .HasForeignKey(b => b.ResourceId)
      .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
