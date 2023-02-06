using MagicVilla_Villa_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_Villa_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(@"Data Source=LocalDatabase.db");
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal",
                    Details = "Here should be some details",
                    Amenity = "Something",
                    Rate = 1000,
                    ImageUrl = "",
                    Occupancy = 5,
                    Sqft = 1000,
                    CreatedAt = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Polton",
                    Details = "Here should be some details",
                    Amenity = "Something",
                    Rate = 500,
                    ImageUrl = "",
                    Occupancy = 3,
                    Sqft = 2000,
                    CreatedAt = DateTime.Now
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Sarantos",
                    Details = "Here should be some details",
                    Amenity = "Something",
                    Rate = 4000,
                    ImageUrl = "",
                    Occupancy = 10,
                    Sqft = 3000,
                    CreatedAt = DateTime.Now
                }
                );
        }
    
    }
}
