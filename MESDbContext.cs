using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.EntitiesConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace SMTS
{
    public class MESDbContext : IdentityDbContext<MyUser>
    {
        public MESDbContext(DbContextOptions<MESDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RunningNumbersConfiguration());
            modelBuilder.ApplyConfiguration(new StockJobOperationRelationConfiguration());
 
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

            //For View Section

            // Map the model class to the database view
            modelBuilder.Entity<JobOperationStatusViewLatest>().ToView("JobOperationStatusViewLatest");
            
            // If the view doesn't have a primary key, you need to define the key for EF Core
             modelBuilder.Entity<JobOperationStatusViewLatest>().HasKey(m => m.Id);

        }

        public DbSet<Types> Type { get; set; }
        public DbSet<PartUOM> PartUOM { get; set; }
        public DbSet<UOMs> UOM { get; set; }
        public DbSet<PartDTL> PartDTL { get; set; }
        public DbSet<PIOT_Counter> PIOTCounter { get; set; }
        public DbSet<PIOTMaintenance> PIOTMaintenance { get; set; }
        public DbSet<PIOTRunning> PIOTRunning { get; set; }
        public DbSet<StockOutIn> StockOutIn { get; set; }
        public DbSet<StockOutInDTL> StockOutInDTL { get; set; }
        public DbSet<StockOutInProperty> StockOutInProperty { get; set; }
        public DbSet<StockLocation> StockLocation { get; set; }
        public DbSet<RunningNumbers> RunningNumbers { get; set; }



        public DbSet<JobOrder> JobOrder { get; set; }
        public DbSet<Color> Color { get; set; }
        //public DbSet<PartDTL> PartDTL { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<JoBOM> JoBOM { get; set; }
        //public DbSet<JobOrderOperationDTO> JobOrderOperation { get; set; }
        public DbSet<JobOrderOperation> JobOrderOperation { get; set; }
        public DbSet<JobOperationStatus> JobOperationStatus { get; set; }
        public DbSet<JobOperationStatusViewLatest> JobOperationStatusViewLatest { get; set; }
        public DbSet<WorkCenter> WorkCenter { get; set; }
        //public DbSet<StockOutInDTL> StockOutInDTL { get; set; }
        public DbSet<Departments> Department { get; set; }
        public DbSet<PartDTL> Part { get; set; }
        //public DbSet<PartUOM> PartUOM { get; set; }
        public DbSet<PartDTLPrefixBatchSN> PartDTLPrefixBatchSN { get; set; }
        public DbSet<PlanningJO> PlanningJO { get; set; }
        public DbSet<StockJobOperationRelation> StockJobOperationRelation { get; set; }

        public DbSet<WeightReadings> WeightReadings { get; set; }
        


    }
}
