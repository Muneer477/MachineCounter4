using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/employee")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _EmployeeService;

    public EmployeeController(IEmployeeService EmployeeService)
    {
        _EmployeeService = EmployeeService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        var Employees = await _EmployeeService.GetAllAsync();
        return Ok(Employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var Employee = await _EmployeeService.GetByIdAsync(id);
        if (Employee == null) return NotFound();
        return Ok(Employee);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var Employee = await _EmployeeService.GetCountAsync();
        return Ok(Employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto EmployeeDto)
    {
        var createdEmployee = await _EmployeeService.CreateAsync(EmployeeDto);
        return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto EmployeeDto)
    {
        if (id != EmployeeDto.Id) return BadRequest();

        var updatedEmployee = await _EmployeeService.UpdateAsync(EmployeeDto);
        if (updatedEmployee == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var success = await _EmployeeService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}


