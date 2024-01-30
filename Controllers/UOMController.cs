using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using SMTS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/UOM")]
[ApiController]
public class UOMController : ControllerBase
{
    private readonly IUOMService _UOMService;

    public UOMController(IUOMService UOMService)
    {
        _UOMService = UOMService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<UOMsDto>>> GetUOMs()
    {
        var uoms = await _UOMService.GetAllAsync();
        return Ok(uoms);
    }   

    [HttpGet("{id}")]
    public async Task<ActionResult<UOMsDto>> GetUOM(int id)
    {
        var uoms = await _UOMService.GetByIdAsync(id);
        if (uoms == null) return NotFound();
        return Ok(uoms);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var uoms = await _UOMService.GetCountAsync();
        return Ok(uoms);
    }

    [HttpPost]
    public async Task<ActionResult<UOMsDto>> CreateUOM(UOMsDto UOMsDto)
    {
        var createdUOM = await _UOMService.CreateAsync(UOMsDto);
        return CreatedAtAction(nameof(GetUOM), new { id = createdUOM.Id }, createdUOM);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUOM(int id, UOMsDto UOMsDto)
    {
        if (id != UOMsDto.Id) return BadRequest();

        var updatedUOM = await _UOMService.UpdateAsync(UOMsDto);
        if (updatedUOM == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUOM(int id)
    {
        var success = await _UOMService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
        

        