using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service.IService;
using SMTS.Services;

namespace SMTS.Controllers
{
    [Route("api/joborder")]
    [ApiController]
    //[Authorize]
    public class JobOrderController : ControllerBase
    {
        private readonly MESDbContext _db;       
        private IMapper _mapper;

        private readonly IJobOrderService _jobOrderService;

        public JobOrderController(IJobOrderService jobOrderService, MESDbContext db, IMapper mapper)
        {
            _jobOrderService = jobOrderService;
            _db = db;
            _mapper = mapper;
        }        

        [HttpGet("list")]
        //[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme )]
        public async Task<ActionResult<IEnumerable<JobOrderDto>>> GetJobOrders()
        {
            var jobOrders = await _jobOrderService.GetAllAsync();
            return Ok(jobOrders);
        }


        //ToListAsync maybe is used to create Id, so cannot
        //public async Task<IEnumerable<JobOrderDto>> GetList()
        //{
        //    var jobOrders = await _db.JobOrder.ToListAsync();
        //    //if (JOs == null)
        //    //    return BadRequest();

        //    return _mapper.Map<IEnumerable<JobOrderDto>>(jobOrders);
        //}

        [HttpGet("count")]
        //[Authorize] // Ensure only authorized users can access this endpoint.
        public async Task<ActionResult<int>> GetCount()
        {
            var jobOrders = await _jobOrderService.GetCountAsync();
            return Ok(jobOrders);
        }
        //public async Task<ActionResult<int>> Count()
        //{
        //    try
        //    {
        //        int count = await _db.JobOrder.CountAsync();

        //        // Check for zero or any specific value if needed
        //        if (count == 0)
        //            return NotFound("No job orders found.");

        //        return Ok(count);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        // Consider using a logging framework like Serilog or NLog
        //        // _logger.LogError(ex, "An error occurred while getting the job order count.");

        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [HttpGet("countNew")]
        //[Authorize] // Ensure only authorized users can access this endpoint.
        public async Task<ActionResult<int>> CountNew()
        {
            try
            {
                int count = await _db.JobOrder.Where(jo => jo.status == "New").CountAsync();

                // Check for zero or any specific value if needed
                if (count == 0)
                    return NotFound("No job orders found.");

                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Consider using a logging framework like Serilog or NLog
                // _logger.LogError(ex, "An error occurred while getting the job order count.");

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("countNewByDate")]
        //[Authorize] // Ensure only authorized users can access this endpoint.
        public async Task<ActionResult<int>> CountNewByDate(DateTime date)
        {
            try
            {
                int count = await _db.JobOrder
                    .Where(jo => jo.status == "New" && jo.SoDate == date.Date) // Assuming you have a CreatedDate property on your JobOrder
                    .CountAsync();

                // Check for zero or any specific value if needed
                if (count == 0)
                    return NotFound("No job orders found for the specified date.");

                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Consider using a logging framework like Serilog or NLog
                // _logger.LogError(ex, "An error occurred while getting the job order count by date.");

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("listnew")]
        public async Task<ActionResult<List<JobOrder>>> ListNew()
        {
            var filteredJOs = await _db.JobOrder.Where(jo => jo.status == "New").ToListAsync();

            if (filteredJOs == null || !filteredJOs.Any())
            {
                return NotFound("No job orders found for the provided start plan date.");
            }

            return Ok(filteredJOs);
        }

        [HttpPost("create")]
        public async Task<ActionResult<JobOrder>> Create([FromBody] JobOrderDto jobOrderDto)
        {
            if (jobOrderDto == null)
            {
                return BadRequest("Invalid data.");
            }

            if (string.IsNullOrEmpty(jobOrderDto.JoNo))
            {
                return BadRequest("JoNo is required.");
            }

            // Convert the DTO to the actual JobOrder model
            var jobOrder = _mapper.Map<JobOrder>(jobOrderDto);

            try
            {
                await _db.JobOrder.AddAsync(jobOrder);
                await _db.SaveChangesAsync();

                // Return the created job order object with its new ID
                return CreatedAtAction(nameof(GetJobOrders), new { id = jobOrder.Id }, jobOrder);
            }
            catch (Exception ex)
            {
                // Ideally, you should log the error here using a logging framework.
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpGet("filterbywcid/{targetJobOrderId:int}")]
        public async Task<ActionResult<List<JobOrder>>> GetJobsByWCID(int targetJobOrderId)
        {
            var workCenterIds = await _db.WorkCenter
          .Where(wc => wc.OperationName == "Blowing")
          .Select(wc => wc.Id)
          .ToListAsync();

            // Then, let's get JobOperationStatus IDs where status is "Planned"
            var plannedStatusIds = await _db.JobOperationStatus
                .Where(jos => jos.Status == "Planned")
                .Select(jos => jos.JobOpId)
                .ToListAsync();

            // Problem in && workCenterIds.Contains(joo.WCId)
            var jobomIds = await _db.JobOrderOperation
                .Where(joo => plannedStatusIds.Contains(joo.Id) && workCenterIds.Contains(joo.WCId))
                .Select(joo => joo.JoBOMId)
                .ToListAsync();

            // Now, let's filter JobOrders
            var jobOrders = await _db.JobOrder
                .Where(jo => jo.Id == targetJobOrderId && _db.JoBOM.Any(jobom => jobomIds.Contains(jobom.Id) && jobom.JoId == jo.Id))
                .Select(jo => new
                {
                    jo.Id,
                    jo.JoNo
                })
                .ToListAsync();

            return Ok(jobomIds);
        }

       // [HttpGet("list-byOperationStatus")]
      //  public async Task <ActionResult<List<>>>
    }
}

