using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/partDTLPrefixBatchSN")]
[ApiController]
public class PartDTLPrefixBatchSNController : ControllerBase
{
    private readonly IPartDTLPrefixBatchSNService _partDTLPrefixBatchSNService;

    public PartDTLPrefixBatchSNController(IPartDTLPrefixBatchSNService partDTLPrefixBatchSNService)
    {
        _partDTLPrefixBatchSNService = partDTLPrefixBatchSNService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PartDTLPrefixBatchSNDto>>> GetPartDTLPrefixBatchSNs()
    {
        var partDTLPrefixBatchSNs = await _partDTLPrefixBatchSNService.GetAllAsync();
        return Ok(partDTLPrefixBatchSNs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartDTLPrefixBatchSNDto>> GetPartDTLPrefixBatchSN(int id)
    {
        var partDTLPrefixBatchSN = await _partDTLPrefixBatchSNService.GetByIdAsync(id);
        if (partDTLPrefixBatchSN == null) return NotFound();
        return Ok(partDTLPrefixBatchSN);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var partDTLPrefixBatchSN = await _partDTLPrefixBatchSNService.GetCountAsync();
        return Ok(partDTLPrefixBatchSN);
    }

    [HttpPost]
    public async Task<ActionResult<PartDTLPrefixBatchSNDto>> CreatePartDTLPrefixBatchSN(PartDTLPrefixBatchSNDto partDTLPrefixBatchSNDto)
    {
        var createdPartDTLPrefixBatchSN = await _partDTLPrefixBatchSNService.CreateAsync(partDTLPrefixBatchSNDto);
        return CreatedAtAction(nameof(GetPartDTLPrefixBatchSN), new { id = createdPartDTLPrefixBatchSN.Id }, createdPartDTLPrefixBatchSN);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePartDTLPrefixBatchSN(int id, PartDTLPrefixBatchSNDto partDTLPrefixBatchSNDto)
    {
        if (id != partDTLPrefixBatchSNDto.Id) return BadRequest();

        var updatedPartDTLPrefixBatchSN = await _partDTLPrefixBatchSNService.UpdateAsync(partDTLPrefixBatchSNDto);
        if (updatedPartDTLPrefixBatchSN == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePartDTLPrefixBatchSN(int id)
    {
        var success = await _partDTLPrefixBatchSNService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


