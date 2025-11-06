using Microsoft.EntityFrameworkCore;
using MLMApp.Models;

namespace MLMApp.Data
{
    /// <summary>
    /// Application database context
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.HasIndex(e => e.MobileNumber).IsUnique();

                // Self-referencing relationship for Sponsor
                // SponsorId (string) references UserId (string)
                entity.HasOne(u => u.Sponsor)
                      .WithMany(u => u.Referrals)
                      .HasForeignKey(u => u.SponsorId)
                      .HasPrincipalKey(u => u.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}

