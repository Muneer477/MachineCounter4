using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs.Location;

namespace SMTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly MESDbContext _context;
        private readonly IMapper mapper;

        public LocationController(MESDbContext context, IMapper mapper)
        {
            this._context = context;
            this.mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<ActionResult<StockLocationDTO[]>> getLocation()
        {
            var locations = await _context.StockLocation.ToListAsync();
            return mapper.Map<StockLocationDTO[]>(locations);
        }
    }
}
