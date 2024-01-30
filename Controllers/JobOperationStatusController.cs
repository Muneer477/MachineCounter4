using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SMTS.DTOs;
using SMTS.DTOs.Stock;
using SMTS.Service;
using SMTS.Service.IService;
using SMTS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/jobOperationStatus")]
[ApiController]
public class JobOperationStatusController : ControllerBase
{
    private readonly IJobOperationStatusService _jobOperationStatusService;
    private readonly IWorkCenterService _workCenterService;
    private readonly IJoBOMService _joBOMService;
    private readonly IJobOrderOperationService _jobOrderOperationService;

    public JobOperationStatusController(IJobOperationStatusService jobOperationStatusService, IJoBOMService joBOMService, IWorkCenterService workCenterService, IJobOrderOperationService jobOrderOperationService)
    {
        _jobOperationStatusService = jobOperationStatusService;
        _workCenterService = workCenterService;
        _jobOrderOperationService = jobOrderOperationService;
        _joBOMService = joBOMService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<JobOperationStatusDTO>>> GetJobOperationStatuss()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();
        return Ok(jobOperationStatuss);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobOperationStatusDTO>> GetJobOperationStatus(int id)
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
    public async Task<ActionResult<JobOperationStatusDTO>> CreateJobOperationStatus(JobOperationStatusDTO jobOperationStatusDTO)
    {
        var createdJobOperationStatus = await _jobOperationStatusService.CreateAsync(jobOperationStatusDTO);
        return CreatedAtAction(nameof(GetJobOperationStatus), new { id = createdJobOperationStatus.Id }, createdJobOperationStatus);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJobOperationStatus(int id, JobOperationStatusDTO jobOperationStatusDTO)
    {
        if (id != jobOperationStatusDTO.Id) return BadRequest();

        var updatedJobOperationStatus = await _jobOperationStatusService.UpdateAsync(jobOperationStatusDTO);
        if (updatedJobOperationStatus == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobOperationStatus(int id)
    {
        var success = await _jobOperationStatusService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpGet("latest/status")]
    public async Task<ActionResult<IEnumerable<JobOperationStatusDTO>>> GetJobOperationStatuss(string status)
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();

        var groupedAndLatest = jobOperationStatuss            
            .GroupBy(j => j.JobOpId)
            .Select(g => g.OrderByDescending(j => j.Datetime).FirstOrDefault())             
            .Where(j => j != null)
            .ToList();

        var filteredByStatus = groupedAndLatest
        .Where(j => j.Status == status)
        .ToList();

        return Ok(filteredByStatus);
    }  

    [HttpGet("latest/status/operation")]
    public async Task<ActionResult<IEnumerable<JobOrderWithOperationStatusDTO>>> GetJobOperationStatusbyOperation(string status, string Operation)
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetByJoStatusOperationAsync(status, Operation);

        return Ok(jobOperationStatuss);
    }

    [HttpGet("list/latest")]
    public async Task<ActionResult<IEnumerable<JobOperationStatusDTO>>> GetJobOperationStatussLatest()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetAllAsync();

        var groupedAndLatest = jobOperationStatuss
            .GroupBy(j => j.JobOpId)
            .Select(g => g.OrderByDescending(j => j.Datetime).FirstOrDefault())
            .Where(j => j != null)
            .ToList();

        //get WCId from JobOrderOperation through JobOpId
        //get OperationName from WorkCenter       
        foreach (var jobOperationStatus in groupedAndLatest)
        {
            var jobOrderOperation = await _jobOrderOperationService.GetByIdAsync(jobOperationStatus.JobOpId);
            if (jobOrderOperation != null)
            {
                jobOperationStatus.WCId = jobOrderOperation.WCId;
                jobOperationStatus.JoBOMId = jobOrderOperation.JoBOMId;

                var workCenter = await _workCenterService.GetByIdAsync(jobOrderOperation.WCId);
                if (workCenter != null)
                {
                    jobOperationStatus.OperationName = workCenter.OperationName;

                    var joBOM = await _joBOMService.GetByIdAsync(jobOrderOperation.JoBOMId);
                    if (joBOM != null)
                    {
                        jobOperationStatus.PartId = joBOM.PartId; //debug cannot get PartId
                    }
                }
            }
        }

        return Ok(groupedAndLatest);
    }




    [HttpGet("view")]
    public async Task<ActionResult<IEnumerable<JobOperationStatusDTO>>> GetJobOperationStatussLatest1()
    {
        var jobOperationStatuss = await _jobOperationStatusService.GetViewDataAsync();

        //var groupedAndLatest = jobOperationStatuss
        //    .GroupBy(j => j.JobOpId)
        //    .Select(g => g.OrderByDescending(j => j.Datetime).FirstOrDefault())
        //    .Where(j => j != null)
        //    .ToList();

        ////get WCId from JobOrderOperation through JobOpId
        ////get OperationName from WorkCenter       
        //foreach (var jobOperationStatus in groupedAndLatest)
        //{
        //    var jobOrderOperation = await _jobOrderOperationService.GetByIdAsync(jobOperationStatus.JobOpId);
        //    if (jobOrderOperation != null)
        //    {
        //        jobOperationStatus.WCId = jobOrderOperation.WCId;

        //        var workCenter = await _workCenterService.GetByIdAsync(jobOrderOperation.WCId);
        //        if (workCenter != null)
        //        {
        //            jobOperationStatus.OperationName = workCenter.OperationName;
        //        }
        //    }
        //}

        //return Ok(groupedAndLatest);

        return Ok(jobOperationStatuss);
    }
}


