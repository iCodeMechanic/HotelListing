using System.ComponentModel.DataAnnotations;
using HotelListing.Entity;

namespace HotelListing.Dtos
{
    public class HotelDto : CreateHotelDto
    {
        public int Id { get; set; }
        public Country Country { get; set; }
    }

    public class UpdateHotelDto : CreateHotelDto
    {

    }
    public class CreateHotelDto
    {
       
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Address { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
