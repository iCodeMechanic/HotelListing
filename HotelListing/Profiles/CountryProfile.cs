using AutoMapper;
using HotelListing.Dtos;
using HotelListing.Entity;

namespace HotelListing.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();
        }
    }
}
