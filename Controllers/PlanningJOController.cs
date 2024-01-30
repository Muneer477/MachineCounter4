using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/planningJO")]
[ApiController]
public class PlanningJOController : ControllerBase
{
    private readonly IPlanningJOService _jobOperationStatusService;

    public PlanningJOController(IPlanningJOService jobOperationStatusService)
    {
        _jobOperationStatusService = jobOperationStatusService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PlanningJODTO>>> GetPlanningJOs()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();
        return Ok(jobOperationStatuss);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlanningJODTO>> GetPlanningJO(int id)
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
    public async Task<ActionResult<PlanningJODTO>> CreatePlanningJO(PlanningJODTO jobOperationStatusDTO)
    {
        var createdPlanningJO = await _jobOperationStatusService.CreateAsync(jobOperationStatusDTO);
        return CreatedAtAction(nameof(GetPlanningJO), new { id = createdPlanningJO.Id }, createdPlanningJO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlanningJO(int id, PlanningJODTO jobOperationStatusDTO)
    {
        if (id != jobOperationStatusDTO.Id) return BadRequest();

        var updatedPlanningJO = await _jobOperationStatusService.UpdateAsync(jobOperationStatusDTO);
        if (updatedPlanningJO == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlanningJO(int id)
    {
        var success = await _jobOperationStatusService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


