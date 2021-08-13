using HotelListing.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "India",
                    ShortName = "IN"
                },
                new Country
                {
                    Id = 2,
                    Name = "United States",
                    ShortName = "USA"
                },
                new Country
                {
                    Id = 3,
                    Name = "United Kingdom",
                    ShortName = "UK"
                }
                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "India Hotel 1",
                    Address = "IN",
                    Rating = 5,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "India Hotel 2",
                    Address = "IN",
                    Rating = 4.7,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 3,
                    Name = "USA Hotel 1",
                    Address = "USA",
                    Rating = 4,
                    CountryId = 2
                },
                new Hotel
                {
                    Id = 4,
                    Name = "USA Hotel 2",
                    Address = "USA",
                    Rating = 4.5,
                    CountryId = 2
                }
            );
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
    }
}
