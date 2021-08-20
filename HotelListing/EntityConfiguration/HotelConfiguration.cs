using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.EntityConfiguration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
    }
}
