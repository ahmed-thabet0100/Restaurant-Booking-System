using Book.Data.Entities;
using Booking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Dtos;
using Restaurant.Data.Entities;

namespace Restaurant.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {

        }

        public DbSet<Book.Data.Entities.Restaurant> Restaurants { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<RestaurantAdminInvitation> RestaurantAdminInvitations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
    .HasOne(c => c.Restaurant)
    .WithMany(r => r.Categories)
    .HasForeignKey(c => c.RestaurantId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Table>()
                .HasKey(t => new { t.TableNumber, t.RestaurantId });
            modelBuilder.Entity<Table>()
                .HasIndex(t => new { t.TableNumber, t.RestaurantId }).IsUnique();

            modelBuilder.Entity<Review>()
               .HasIndex(r => new { r.RestaurantId, r.UserId })
               .IsUnique();

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Table)
                .WithMany()
                .HasForeignKey(r => new { r.TableNumber, r.RestaurantId })
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
    .HasOne(r => r.Restaurant)
    .WithMany()
    .HasForeignKey(r => r.RestaurantId)
    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Reservation>()
            //    .HasOne(r => r.User)
            //    .WithMany()
            //    .HasForeignKey(r => r.UserId)
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion(
                 Rstatus => Rstatus.ToString(),
                 Rstatus => (ReservationStatus)Enum.Parse(typeof(ReservationStatus), Rstatus));

            base.OnModelCreating(modelBuilder);
        }
    }
}
