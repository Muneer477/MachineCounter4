using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/jobOrderOperation")]
[ApiController]
public class JobOrderOperationController : ControllerBase
{
    private readonly IJobOrderOperationService _jobOrderOperationService;

    public JobOrderOperationController(IJobOrderOperationService jobOrderOperationService)
    {
        _jobOrderOperationService = jobOrderOperationService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<JobOrderOperationDTO>>> GetJobOrderOperations()
    {
        var jobOrderOperations = await _jobOrderOperationService.GetAllAsync();
        return Ok(jobOrderOperations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobOrderOperationDTO>> GetJobOrderOperation(int id)
    {
        var jobOrderOperation = await _jobOrderOperationService.GetByIdAsync(id);
        if (jobOrderOperation == null) return NotFound();
        return Ok(jobOrderOperation);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var jobOrderOperation = await _jobOrderOperationService.GetCountAsync();
        return Ok(jobOrderOperation);
    }

    [HttpPost]
    public async Task<ActionResult<JobOrderOperationDTO>> CreateJobOrderOperation(JobOrderOperationDTO jobOrderOperationDTO)
    {
        var createdJobOrderOperation = await _jobOrderOperationService.CreateAsync(jobOrderOperationDTO);
        return CreatedAtAction(nameof(GetJobOrderOperation), new { id = createdJobOrderOperation.Id }, createdJobOrderOperation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJobOrderOperation(int id, JobOrderOperationDTO jobOrderOperationDTO)
    {
        if (id != jobOrderOperationDTO.Id) return BadRequest();

        var updatedJobOrderOperation = await _jobOrderOperationService.UpdateAsync(jobOrderOperationDTO);
        if (updatedJobOrderOperation == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobOrderOperation(int id)
    {
        var success = await _jobOrderOperationService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


