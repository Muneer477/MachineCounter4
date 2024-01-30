using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMTS.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SMTS
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 
            // Seed user data
            var hasher = new PasswordHasher<MyUser>();
            modelBuilder.Entity<MyUser>().HasData(
                new MyUser
                {
                    Id = "1", // Replace with a valid ID
                    UserName = "ADMIN",
                    NormalizedUserName = "ADMIN",
                    Email = "ADMIN@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "ADMIN!"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Name = "ADMIN"
                }
            );

    

            modelBuilder.Entity<StockOutIn>().HasKey(s => s.DocKey);

            modelBuilder.Entity<PartDTL>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd(); // This is enough to set it as an identity column with default identity settings
            });

            modelBuilder.Entity<PartUOM>()
                .Property(p => p.Rate).HasColumnType("decimal(18, 2)");

        }
        //public DbSet<JobOrder> JobOrders { get; set; }
        
    }
}
