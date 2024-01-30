using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service;
using SMTS.Service.IService;
using SMTS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SMTS.Controllers
{
    [Route("api/WeightReading")]
    [ApiController]
    public class WeightReadingController : ControllerBase
    {
        private readonly MESDbContext _db;
        private IMapper _mapper;

        public WeightReadingController(MESDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<WeightReadings>> Create([FromBody] WeightReadingsDto weightReadingsDto)
        {
            if (weightReadingsDto == null)
            {
                return BadRequest("Invalid data.");
            }


            // Convert the DTO to the actual JobOrder model
            var weightReading = _mapper.Map<WeightReadings>(weightReadingsDto);

            try
            {
                await _db.WeightReadings.AddAsync(weightReading);
                await _db.SaveChangesAsync();

                // Return the created job order object with its new ID
                return CreatedAtAction(nameof(GetList), new { id = weightReading.Id }, weightReading);
            }
            catch (Exception ex)
            {
                // Ideally, you should log the error here using a logging framework.
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("GetHighestId")]
        public async Task<ActionResult<WeightReadings>> GetWeightReadingsWithHighestId()
        {
            try
            {
                // Retrieve the WeightReadings with the highest ID
                var highestIdWeightReading = await _db.WeightReadings
                    .OrderByDescending(w => w.Id)
                    .FirstOrDefaultAsync();

                if (highestIdWeightReading == null)
                {
                    return NotFound("No WeightReadings found.");
                }

                return Ok(highestIdWeightReading);
            }
            catch (Exception ex)
            {
                // Ideally, you should log the error here using a logging framework.
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<WeightReadings>>> GetList()
        {
            var WR = await this._db.WeightReadings.ToListAsync();
            if (WR == null)
                return BadRequest();

            return WR;
        }

    }
}

