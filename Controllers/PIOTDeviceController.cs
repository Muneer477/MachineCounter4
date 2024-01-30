using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SMTS;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/piotCounter")]
[ApiController]
public class PIOTCounterController : ControllerBase
{
    private readonly IPIOTCounterService _jobOperationStatusService;
    private readonly MESDbContext _context;

    public PIOTCounterController(IPIOTCounterService jobOperationStatusService, MESDbContext context)
    {
        _jobOperationStatusService = jobOperationStatusService;
        this._context = context;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PIOTCounterDTO>>> GetPIOTCounters()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();
        return Ok(jobOperationStatuss);
    }

    [HttpGet("counter")]
    public async Task<IActionResult> CreateCounter(string serialNo, string ipAddress)
    {
        if (string.IsNullOrEmpty(serialNo) || string.IsNullOrEmpty(ipAddress))
        {
            return BadRequest("Serial number and IP address must be provided.");
        }

        try
        {
            var pIOTMaintainance = await _context.PIOTMaintenance
                .Where(x => x.SerialNumber == serialNo)
                .FirstOrDefaultAsync();

            if (pIOTMaintainance == null)
            {
                return NotFound("Serial number not found.");
            }

            var pIOT_Counter = new PIOT_Counter()
            {
                IOTDeviceId = pIOTMaintainance.Id,
                MachineNo = pIOTMaintainance.MachineNo,
                IpAddress = ipAddress,
                Time = DateTime.Now
            };

            await _context.AddAsync(pIOT_Counter);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            // Handle the exception, log it, and return an appropriate error response.
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<PIOTCounterDTO>> GetPIOTCounter(int id)
    {
        var jobOperationStatus = await _jobOperationStatusService.GetByIdAsync(id);
        if (jobOperationStatus == null) return NotFound();
        return Ok(jobOperationStatus);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var jobOperationStatus = await _jobOperationStatusService.GetCountAsync();
        return Ok(jobOperationStatus);
    }

    [HttpPost]
    public async Task<ActionResult<PIOTCounterDTO>> CreatePIOTCounter(PIOTCounterDTO jobOperationStatusDTO)
    {
        var createdPIOTCounter = await _jobOperationStatusService.CreateAsync(jobOperationStatusDTO);
        return CreatedAtAction(nameof(GetPIOTCounter), new { id = createdPIOTCounter.Id }, createdPIOTCounter);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePIOTCounter(int id, PIOTCounterDTO jobOperationStatusDTO)
    {
        if (id != jobOperationStatusDTO.Id) return BadRequest();

        var updatedPIOTCounter = await _jobOperationStatusService.UpdateAsync(jobOperationStatusDTO);
        if (updatedPIOTCounter == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePIOTCounter(int id)
    {
        var success = await _jobOperationStatusService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}

[Route("api/piotRunning")]
[ApiController]
public class PIOTRunningController : ControllerBase
{
    private readonly IPIOTRunningService _jobOperationStatusService;
    private readonly MESDbContext _context;

    public PIOTRunningController(IPIOTRunningService jobOperationStatusService, MESDbContext context)
    {
        _jobOperationStatusService = jobOperationStatusService;
        this._context = context;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PIOTRunningDTO>>> GetPIOTRunnings()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();
        return Ok(jobOperationStatuss);
    }

    [HttpGet("running")]
    public async Task<IActionResult> CreateRunning(string serialNo, string ipAddress)
    {
        if (string.IsNullOrEmpty(serialNo) || string.IsNullOrEmpty(ipAddress))
        {
            return BadRequest("Serial number and IP address must be provided.");
        }

        try
        {
            var pIOTMaintainance = await _context.PIOTMaintenance
                .Where(x => x.SerialNumber == serialNo)
                .FirstOrDefaultAsync();

            if (pIOTMaintainance == null)
            {
                return NotFound("Serial number not found.");
            }

            var pIOT_Running = new PIOTRunning()
            {
                IOTDeviceId = pIOTMaintainance.Id,
                MachineNo = pIOTMaintainance.MachineNo,
                IpAddress = ipAddress,
                Time = DateTime.Now
            };

            await _context.AddAsync(pIOT_Running);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            // Handle the exception, log it, and return an appropriate error response.
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<PIOTRunningDTO>> GetPIOTRunning(int id)
    {
        var jobOperationStatus = await _jobOperationStatusService.GetByIdAsync(id);
        if (jobOperationStatus == null) return NotFound();
        return Ok(jobOperationStatus);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var jobOperationStatus = await _jobOperationStatusService.GetCountAsync();
        return Ok(jobOperationStatus);
    }

    [HttpPost]
    public async Task<ActionResult<PIOTRunningDTO>> CreatePIOTRunning(PIOTRunningDTO jobOperationStatusDTO)
    {
        var createdPIOTRunning = await _jobOperationStatusService.CreateAsync(jobOperationStatusDTO);
        return CreatedAtAction(nameof(GetPIOTRunning), new { id = createdPIOTRunning.Id }, createdPIOTRunning);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePIOTRunning(int id, PIOTRunningDTO jobOperationStatusDTO)
    {
        if (id != jobOperationStatusDTO.Id) return BadRequest();

        var updatedPIOTRunning = await _jobOperationStatusService.UpdateAsync(jobOperationStatusDTO);
        if (updatedPIOTRunning == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePIOTRunning(int id)
    {
        var success = await _jobOperationStatusService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}




[Route("api/piotMaintenance")]
[ApiController]
public class PIOTMaintenanceController : ControllerBase
{
    private readonly IPIOTMaintainanceService _piotMaintenanceService;
    private readonly MESDbContext _context;

    public PIOTMaintenanceController(IPIOTMaintainanceService piotMaintenanceService, MESDbContext context)
    {
        _piotMaintenanceService = piotMaintenanceService;
        this._context = context;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PIOTMaintainanceDTO>>> GetPIOTMaintenances()
    {
        var piotMaintenances = await _piotMaintenanceService.GetAllAsync();
        return Ok(piotMaintenances);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PIOTMaintainanceDTO>> GetPIOTMaintenance(int id)
    {
        var piotMaintenance = await _piotMaintenanceService.GetByIdAsync(id);
        if (piotMaintenance == null) return NotFound();
        return Ok(piotMaintenance);
    }

    [HttpPost]
    public async Task<ActionResult<PIOTMaintainanceDTO>> CreatePIOTMaintenance(PIOTMaintainanceDTO piotMaintenanceDTO)
    {
        var createdPIOTMaintenance = await _piotMaintenanceService.CreateAsync(piotMaintenanceDTO);
        return CreatedAtAction(nameof(GetPIOTMaintenance), new { id = createdPIOTMaintenance.Id }, createdPIOTMaintenance);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePIOTMaintenance(int id, PIOTMaintainanceDTO piotMaintenanceDTO)
    {
        if (id != piotMaintenanceDTO.Id) return BadRequest();

        var updatedPIOTMaintenance = await _piotMaintenanceService.UpdateAsync(piotMaintenanceDTO);
        if (updatedPIOTMaintenance == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePIOTMaintenance(int id)
    {
        var success = await _piotMaintenanceService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }


}