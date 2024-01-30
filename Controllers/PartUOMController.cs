using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/partUOM")]
[ApiController]
public class PartUOMController : ControllerBase
{
    private readonly IPartUOMService _partUOMService;

    public PartUOMController(IPartUOMService partUOMService)
    {
        _partUOMService = partUOMService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<PartUOMDto>>> GetPartUOMs()
    {
        var partUOMs = await _partUOMService.GetAllAsync();
        return Ok(partUOMs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartUOMDto>> GetPartUOM(int id)
    {
        var partUOM = await _partUOMService.GetByIdAsync(id);
        if (partUOM == null) return NotFound();
        return Ok(partUOM);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var partUOM = await _partUOMService.GetCountAsync();
        return Ok(partUOM);
    }

    [HttpPost]
    public async Task<ActionResult<PartUOMDto>> CreatePartUOM(PartUOMDto partUOMDto)
    {
        var createdPartUOM = await _partUOMService.CreateAsync(partUOMDto);
        return CreatedAtAction(nameof(GetPartUOM), new { id = createdPartUOM.Id }, createdPartUOM);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePartUOM(int id, PartUOMDto partUOMDto)
    {
        if (id != partUOMDto.Id) return BadRequest();

        var updatedPartUOM = await _partUOMService.UpdateAsync(partUOMDto);
        if (updatedPartUOM == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePartUOM(int id)
    {
        var success = await _partUOMService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


