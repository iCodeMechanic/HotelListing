﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.Dtos;
using HotelListing.Entity;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                return Ok(_mapper.Map<IList<CountryDto>>(countries));
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something went wrong in the {nameof(GetCountries)}");
                return BadRequest("Server Error");
            }
        }
        [Authorize]
        [HttpGet("{id:int}", Name = "GetCountry")]
        
        public async Task<ActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(x=>x.Id == id,new List<string>{"Hotels"});
                return Ok(_mapper.Map<CountryDto>(country));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                return BadRequest("Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateCountry(CreateCountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDto);

                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateCountry)}");
                return BadRequest("Server Error");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCountry(int id, UpdateCountryDto countryDto)
        {
            if (!ModelState.IsValid && id > 1)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWork.Countries.Get(x => x.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Something went wrong in the {nameof(UpdateCountry)}");
                    return BadRequest("Submit Data is invalid");
                }

                country = _mapper.Map(countryDto, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");
                return BadRequest("Server Error");
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in the {nameof(DeleteCountry)}");
                return BadRequest();
            }

            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in the {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                return BadRequest("Server Error");
            }
        }
    }
}
