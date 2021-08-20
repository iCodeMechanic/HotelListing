using System;
using System.Text;
using HotelListing.Data;
using HotelListing.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HotelListing.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builders 
                = services.AddIdentityCore<ApiUser>(
                    x=>x.User.RequireUniqueEmail = true);

            builders = new IdentityBuilder(builders.UserType, typeof(IdentityRole), services);
            builders.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("KEY");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            //services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //    {
            //        o.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            //ValidateIssuer = true,
            //            //ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            //ValidIssuer = jwtSettings.GetSection("Issuer").Value,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            //        };
            //    });
        }
    }
}
