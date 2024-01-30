using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/department")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var department = await _departmentService.GetCountAsync();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto departmentDto)
    {
        var createdDepartment = await _departmentService.CreateAsync(departmentDto);
        return CreatedAtAction(nameof(GetDepartment), new { id = createdDepartment.Id }, createdDepartment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, DepartmentDto departmentDto)
    {
        if (id != departmentDto.Id) return BadRequest();

        var updatedDepartment = await _departmentService.UpdateAsync(departmentDto);
        if (updatedDepartment == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var success = await _departmentService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


