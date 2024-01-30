using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/workcenter")]
[ApiController]
public class WorkCenterController : ControllerBase
{
    private readonly IWorkCenterService _WorkCenterService;

    public WorkCenterController(IWorkCenterService WorkCenterService)
    {
        _WorkCenterService = WorkCenterService;
    }
    

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<WorkCenterDto>>> GetWorkCenters()
    {
        var WorkCenters = await _WorkCenterService.GetAllAsync();
        return Ok(WorkCenters);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkCenterDto>> GetWorkCenter(int id)
    {
        var WorkCenter = await _WorkCenterService.GetByIdAsync(id);
        if (WorkCenter == null) return NotFound();
        return Ok(WorkCenter);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var WorkCenter = await _WorkCenterService.GetCountAsync();
        return Ok(WorkCenter);
    }

    [HttpPost]
    public async Task<ActionResult<WorkCenterDto>> CreateWorkCenter(WorkCenterDto WorkCenterDto)
    {
        var createdWorkCenter = await _WorkCenterService.CreateAsync(WorkCenterDto);
        return CreatedAtAction(nameof(GetWorkCenter), new { id = createdWorkCenter.Id }, createdWorkCenter);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkCenter(int id, WorkCenterDto WorkCenterDto)
    {
        if (id != WorkCenterDto.Id) return BadRequest();

        var updatedWorkCenter = await _WorkCenterService.UpdateAsync(WorkCenterDto);
        if (updatedWorkCenter == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkCenter(int id)
    {
        var success = await _WorkCenterService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


