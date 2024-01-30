using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.Service;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

[Route("api/contact")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
        
    [HttpGet("list")]
    //[Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme )]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
    {
        var contacts = await _contactService.GetAllAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(int id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        if (contact == null) return NotFound();
        return Ok(contact);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var contact = await _contactService.GetCountAsync();
        return Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(ContactDto contactDto)
    {
        var createdContact = await _contactService.CreateAsync(contactDto);
        return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, ContactDto contactDto)
    {
        if (id != contactDto.Id) return BadRequest();

        var updatedContact = await _contactService.UpdateAsync(contactDto);
        if (updatedContact == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var success = await _contactService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
        

        