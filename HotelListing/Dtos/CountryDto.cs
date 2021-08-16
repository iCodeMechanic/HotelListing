using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Dtos
{
    public class CountryDto : CreateCountryDto
    {
        public int Id { get; set; }
        public virtual IList<HotelDto> Hotels { get; set; }

    }
    public class CreateCountryDto
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(maximumLength: 2)]
        public string ShortName { get; set; }
    }
}
