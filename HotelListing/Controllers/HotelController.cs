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
        public async Task<ActionResult> GetHotels([FromQuery] RequestParams requestParams)
        {
            
                var hotels = await _unitOfWork.Hotels.GetPagedListAll(requestParams);
                return Ok(_mapper.Map<IList<HotelDto>>(hotels));

            
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        public async Task<ActionResult> GetHotel(int id)
        {
           
                var hotel = await _unitOfWork.Hotels.Get(x => x.Id == id, new List<string> { "Country" });
                return Ok(_mapper.Map<HotelDto>(hotel));
            
        }

        [HttpPost]
        public async Task<ActionResult> CreateHotel(CreateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError( $"Something went wrong in the {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            
            var hotel = _mapper.Map<Hotel>(hotelDto);

            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotel",new {id = hotel.Id},hotel);
           
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateHotel(int id, UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid && id > 1)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            
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

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in the {nameof(CreateHotel)}");
                return BadRequest();
            }

            
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
        

        

    }
}
