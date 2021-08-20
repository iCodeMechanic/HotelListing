using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Dtos;

namespace HotelListing.IRepository
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<string> CreateToken();
    }
}
