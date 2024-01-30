using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs;
using SMTS.DTOs.Types;
using SMTS.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMTS.Controllers
{
    [Route("api/gm")]
    [ApiController]
    public class GeneralMaintenanceController : ControllerBase
    {
        private readonly MESDbContext db;
        private readonly IMapper mapper;

        public GeneralMaintenanceController(MESDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<TypesDTO>>> Get()
        {
            var types = await db.Type.ToListAsync();
            return mapper.Map<List<TypesDTO>>(types);
        }




    }
}
