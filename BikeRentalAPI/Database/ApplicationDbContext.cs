using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalAPI.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserInfo>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Bike>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Station>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<RentalSchedule>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
        }
        public DbSet<UserInfo> UserInfos { get; set; } = null;
        public DbSet<Bike> Bikes { get; set; } = null;
        public DbSet<Station> Stations { get; set; } = null;
        
        public DbSet<RentalSchedule> RentalSchedules { get; set; } = null;
    }
}
