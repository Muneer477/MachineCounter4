using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/stockJobOperationRelation")]
[ApiController]
public class StockJobOperationRelationController : ControllerBase
{
    private readonly IStockJobOperationRelationService _stockJobOperationRelationService;

    public StockJobOperationRelationController(IStockJobOperationRelationService stockJobOperationRelationService)
    {
        _stockJobOperationRelationService = stockJobOperationRelationService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<StockJobOperationRelationDTO>>> GetStockJobOperationRelations()
    {
        var stockJobOperationRelations = await _stockJobOperationRelationService.GetAllAsync();
        return Ok(stockJobOperationRelations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StockJobOperationRelationDTO>> GetStockJobOperationRelation(int id)
    {
        var stockJobOperationRelation = await _stockJobOperationRelationService.GetByIdAsync(id);
        if (stockJobOperationRelation == null) return NotFound();
        return Ok(stockJobOperationRelation);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var stockJobOperationRelation = await _stockJobOperationRelationService.GetCountAsync();
        return Ok(stockJobOperationRelation);
    }

    [HttpPost]
    public async Task<ActionResult<StockJobOperationRelationDTO>> CreateStockJobOperationRelation(StockJobOperationRelationDTO stockJobOperationRelationDTO)
    {
        var createdStockJobOperationRelation = await _stockJobOperationRelationService.CreateAsync(stockJobOperationRelationDTO);
        return CreatedAtAction(nameof(GetStockJobOperationRelation), new { JobOperationId = createdStockJobOperationRelation.JobOperationId }, createdStockJobOperationRelation);
    }

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateStockJobOperationRelation(int id, StockJobOperationRelationDTO stockJobOperationRelationDTO)
    //{
    //    if (id != stockJobOperationRelationDTO.Id) return BadRequest();

    //    var updatedStockJobOperationRelation = await _stockJobOperationRelationService.UpdateAsync(stockJobOperationRelationDTO);
    //    if (updatedStockJobOperationRelation == null) return NotFound();
    //    return NoContent();
    //}

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStockJobOperationRelation(int id)
    {
        var success = await _stockJobOperationRelationService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


