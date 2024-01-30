using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.DTOs.Stock;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/stockOutInDTL")]
[ApiController]
public class StockOutInDTLController : ControllerBase
{
    private readonly IStockOutInDTLService _stockOutInDTLService;

    public StockOutInDTLController(IStockOutInDTLService stockOutInDTLService)
    {
        _stockOutInDTLService = stockOutInDTLService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<StockOutInDTLDTO>>> GetStockOutInDTLs()
    {
        var stockOutInDTLs = await _stockOutInDTLService.GetAllAsync();
        return Ok(stockOutInDTLs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StockOutInDTLDTO>> GetStockOutInDTL(int id)
    {
        var stockOutInDTL = await _stockOutInDTLService.GetByIdAsync(id);
        if (stockOutInDTL == null) return NotFound();
        return Ok(stockOutInDTL);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var stockOutInDTL = await _stockOutInDTLService.GetCountAsync();
        return Ok(stockOutInDTL);
    }

    [HttpPost]
    public async Task<ActionResult<StockOutInDTLDTO>> CreateStockOutInDTL(StockOutInDTLDTO stockOutInDTLDto)
    {
        var createdStockOutInDTL = await _stockOutInDTLService.CreateAsync(stockOutInDTLDto);
        return CreatedAtAction(nameof(GetStockOutInDTL), new { id = createdStockOutInDTL.DtlKey }, createdStockOutInDTL);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStockOutInDTL(int id, StockOutInDTLDTO stockOutInDTLDto)
    {
        if (id != stockOutInDTLDto.DtlKey) return BadRequest();

        var updatedStockOutInDTL = await _stockOutInDTLService.UpdateAsync(stockOutInDTLDto);
        if (updatedStockOutInDTL == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStockOutInDTL(int id)
    {
        var success = await _stockOutInDTLService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpGet("get-blowing-roll")]
    public async Task<ActionResult<BlowingRollSuffixDTO>> getBlowingRoll(string Jono)
    {
        var data = await _stockOutInDTLService.getRollSuffixAsync(Jono);
        return Ok(data);
    }
}


