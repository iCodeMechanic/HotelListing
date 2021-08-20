using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.Dtos;
using HotelListing.Entity;
using HotelListing.IRepository;
using Microsoft.Extensions.Logging;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                return Ok(_mapper.Map<IList<HotelDto>>(hotels));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return BadRequest("Server Error");
            }
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        public async Task<ActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(x => x.Id == id, new List<string> { "Country" });
                return Ok(_mapper.Map<HotelDto>(hotel));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}");
                return BadRequest("Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateHotel(CreateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError( $"Something went wrong in the {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel",new {id = hotel.Id},hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
                return BadRequest("Server Error");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateHotel(int id, UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid && id > 1)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(x=>x.Id == id);
                if (hotel == null)
                {
                    _logger.LogError( $"Something went wrong in the {nameof(UpdateHotel)}");
                    return BadRequest("Submit Data is invalid");
                }

                hotel = _mapper.Map(hotelDto, hotel);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
                return BadRequest("Server Error");
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in the {nameof(CreateHotel)}");
                return BadRequest();
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in the {nameof(CreateHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return BadRequest("Server Error");
            }
        }
        

        

    }
}
