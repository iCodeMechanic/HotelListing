using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HotelListing.Dtos;
using HotelListing.Entity;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
       // private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager,ILogger<AccountController> logger, IMapper mapper,IAuthManager authManager)
        {
            _userManager = userManager;
           // _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}");
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user,userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code,error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRoleAsync(user, userDto.Role);
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem("Server Error");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"Login attempt for {loginDto.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authManager.ValidateUser(loginDto))
                {
                    return Unauthorized();
                }

                return Ok(new { Token = await _authManager.CreateToken() });

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                return Problem("Server Error");
            }
        }

    }
}
