using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.Dtos;
using HotelListing.Entity;

namespace HotelListing.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApiUser, UserDto>().ReverseMap();
            CreateMap<ApiUser, LoginDto>().ReverseMap();
        }
       
    }
}
