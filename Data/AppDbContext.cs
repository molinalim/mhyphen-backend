using mhyphen.Models;
using Microsoft.EntityFrameworkCore;

namespace mhyphen.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Movie)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MovieId);
        }
    }
}