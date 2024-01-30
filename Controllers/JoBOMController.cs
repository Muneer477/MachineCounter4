using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/joBOM")]
[ApiController]
public class JoBOMController : ControllerBase
{
    private readonly IJoBOMService _jobOperationStatusService;

    public JoBOMController(IJoBOMService jobOperationStatusService)
    {
        _jobOperationStatusService = jobOperationStatusService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<JoBOMDTO>>> GetJoBOMs()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();
        return Ok(jobOperationStatuss);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JoBOMDTO>> GetJoBOM(int id)
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
    public async Task<ActionResult<JoBOMDTO>> CreateJoBOM(JoBOMDTO jobOperationStatusDTO)
    {
        var createdJoBOM = await _jobOperationStatusService.CreateAsync(jobOperationStatusDTO);
        return CreatedAtAction(nameof(GetJoBOM), new { id = createdJoBOM.Id }, createdJoBOM);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJoBOM(int id, JoBOMDTO jobOperationStatusDTO)
    {
        if (id != jobOperationStatusDTO.Id) return BadRequest();

        var updatedJoBOM = await _jobOperationStatusService.UpdateAsync(jobOperationStatusDTO);
        if (updatedJoBOM == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJoBOM(int id)
    {
        var success = await _jobOperationStatusService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


