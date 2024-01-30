using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/part")]
[ApiController]
public class PartController : ControllerBase
{
    private readonly IPartService _partService;

    public PartController(IPartService partService)
    {
        _partService = partService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PartDto>>> GetParts()
    {
        var parts = await _partService.GetAllAsync();
        return Ok(parts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartDto>> GetPart(int id)
    {
        var part = await _partService.GetByIdAsync(id);
        if (part == null) return NotFound();
        return Ok(part);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var part = await _partService.GetCountAsync();
        return Ok(part);
    }

    [HttpPost]
    public async Task<ActionResult<PartDto>> CreatePart(PartDto partDto)
    {
        var createdPart = await _partService.CreateAsync(partDto);
        return CreatedAtAction(nameof(GetPart), new { id = createdPart.Id }, createdPart);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePart(int id, PartDto partDto)
    {
        if (id != partDto.Id) return BadRequest();

        var updatedPart = await _partService.UpdateAsync(partDto);
        if (updatedPart == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        var success = await _partService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


