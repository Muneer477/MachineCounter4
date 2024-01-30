using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

using SMTS.DTOs.Stock;

[Route("api/stockOutIn")]
[ApiController]
public class StockOutInController : ControllerBase
{
    private readonly IStockOutInService _jobOperationStatusService;

    public StockOutInController(IStockOutInService jobOperationStatusService)
    {
        _jobOperationStatusService = jobOperationStatusService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<StockOutInDTO>>> GetStockOutIns()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();
        return Ok(jobOperationStatuss);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StockOutInDTO>> GetStockOutIn(int id)
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
    public async Task<ActionResult<StockOutInDTO>> CreateStockOutIn(StockOutInDTO jobOperationStatusDTO)
    {
        var createdStockOutIn = await _jobOperationStatusService.CreateAsync(jobOperationStatusDTO);
        return CreatedAtAction(nameof(GetStockOutIn), new { id = createdStockOutIn.Id }, createdStockOutIn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStockOutIn(int id, StockOutInDTO jobOperationStatusDTO)
    {
        if (id != jobOperationStatusDTO.Id) return BadRequest();

        var updatedStockOutIn = await _jobOperationStatusService.UpdateAsync(jobOperationStatusDTO);
        if (updatedStockOutIn == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStockOutIn(int id)
    {
        var success = await _jobOperationStatusService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


