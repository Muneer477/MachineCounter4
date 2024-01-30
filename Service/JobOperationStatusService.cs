using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMTS.Utility;
using SMTS.DTOs.Stock;

namespace SMTS.Services
{
    public class JobOperationStatusService : IJobOperationStatusService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWorkCenterService _workCenterService;
        private readonly IJobOrderOperationService _jobOrderOperationService;
        private readonly IJobOrderService _jobOrderService;
        private readonly IJoBOMService _joBOMService;

        public JobOperationStatusService(MESDbContext context, IMapper mapper, IWorkCenterService workCenterService, IJobOrderOperationService jobOrderOperationService, IJobOrderService jobOrderService, IJoBOMService joBOMService)
        {
            _context = context;
            _mapper = mapper;
            this._workCenterService = workCenterService;
            this._jobOrderOperationService = jobOrderOperationService;
            _jobOrderService = jobOrderService;
            this._joBOMService = joBOMService;
        }



        public async Task<IEnumerable<JobOperationStatusDTO>> GetAllAsync()
        {
            var jobOperationStatuss = await _context.JobOperationStatus.ToListAsync();
            return _mapper.Map<IEnumerable<JobOperationStatusDTO>>(jobOperationStatuss);
        }

        public async Task<JobOperationStatusDTO?> GetByIdAsync(int id)
        {
            var jobOperationStatus = await _context.JobOperationStatus.FindAsync(id);
            return _mapper.Map<JobOperationStatusDTO>(jobOperationStatus);
        }

        public async Task<JobOperationStatusDTO?> CreateAsync(JobOperationStatusDTO jobOperationStatusDTO)
        {
            var jobOperationStatus = _mapper.Map<JobOperationStatus>(jobOperationStatusDTO);
            _context.JobOperationStatus.Add(jobOperationStatus);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobOperationStatusDTO>(jobOperationStatus);
        }

        public async Task<JobOperationStatusDTO> UpdateAsync(JobOperationStatusDTO jobOperationStatusDTO)
        {
            try
            {
                var jobOperationStatus = await _context.JobOperationStatus.FindAsync(jobOperationStatusDTO.Id);
                if (jobOperationStatus == null)
                {
                    // You need to decide what to do if the jobOperationStatus isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("JobOperationStatus not found.");
                }

                _mapper.Map(jobOperationStatusDTO, jobOperationStatus);
                _context.JobOperationStatus.Update(jobOperationStatus);
                await _context.SaveChangesAsync();
                return _mapper.Map<JobOperationStatusDTO>(jobOperationStatus); // Map and return the updated jobOperationStatus
            }
            catch (DbUpdateException ex)
            {
                if (IsForeignKeyViolation(ex))
                {
                    // Handle the foreign key violation
                    throw new CustomException("Foreign key constraint violated.");
                }
                else
                {
                    // Handle other types of DbUpdateException or rethrow
                    // Depending on your use case, you might want to return a default value, null, or throw
                    throw; // Rethrows the current exception
                }
            }
            // If there are other potential exceptions that should be caught and handled differently,
            // add additional catch blocks here
        }
        private bool IsForeignKeyViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as SqlException;

            if (sqlException != null)
            {
                foreach (SqlError error in sqlException.Errors)
                {
                    // In SQL Server, the number for a foreign key violation is 547
                    if (error.Number == 547)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobOperationStatus = await _context.JobOperationStatus.FindAsync(id);
            if (jobOperationStatus == null) return false;

            _context.JobOperationStatus.Remove(jobOperationStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.JobOperationStatus.CountAsync();
            return count;
        }

        public async Task<List<JobOrderWithOperationStatusDTO>> GetByJoStatusOperationAsync(string status, string Operation)
        {
            var jobOperationStatuss = await GetAllAsync();
            var WorkCenters = await _workCenterService.GetAllAsync();
            var jobOperations = await _jobOrderOperationService.GetAllAsync();
            var WorkCenterIds = WorkCenters.Where(row => row.OperationName == Operation).Select(row => row.Id).ToList();
            var jobOperationIds = jobOperations.Where(row => WorkCenterIds.Contains(row.WCId)).Select(row => row.Id).ToList();
            var groupedAndLatest = jobOperationStatuss
                .GroupBy(j => j.JobOpId)
                .Select(g => g.OrderByDescending(j => j.Datetime).FirstOrDefault())
                .Where(j => j != null)
                .ToList();
            var filteredByStatusOperation = groupedAndLatest
            .Where(j => j.Status == status && jobOperationIds.Contains(j.JobOpId))
            .ToList();
            var JobOrderWithOperationStatusList = new List<JobOrderWithOperationStatusDTO>();
            foreach (var f in filteredByStatusOperation)
            {
                var JobOrderInfoByOperation = await _jobOrderService.getJobOrderInfoByOperationId(f.JobOpId);
                var PartId = await _joBOMService.GetPartIdByJobOperationId(f.JobOpId);
                if (JobOrderInfoByOperation != null)
                {
                    var JobOrderWithOperationStatus = _mapper.Map<JobOrderWithOperationStatusDTO>(f);
                    JobOrderWithOperationStatus.JoId = JobOrderInfoByOperation.Id;
                    JobOrderWithOperationStatus.JoNo = JobOrderInfoByOperation.JoNo;
                    JobOrderWithOperationStatus.partId = PartId;
                    JobOrderWithOperationStatusList.Add(JobOrderWithOperationStatus);
                }
            }
            return JobOrderWithOperationStatusList;
        }
               

        public async Task<IEnumerable<JobOperationStatusDTO>> GetAllWithDetailsAsync()
        {
            // Initial mapping
            var jobOperationStatuses = await _context.JobOperationStatus.ToListAsync();
            var jobOperationStatusDTOs = _mapper.Map<IEnumerable<JobOperationStatusDTO>>(jobOperationStatuses);

            // Additional mapping logic handled here
            foreach (var dto in jobOperationStatusDTOs)
            {
                var jobOrderOperation = await _jobOrderOperationService.GetByIdAsync(dto.JobOpId);
                if (jobOrderOperation != null)
                {
                    dto.WCId = jobOrderOperation.WCId;

                    var workCenter = await _workCenterService.GetByIdAsync(jobOrderOperation.WCId);
                    if (workCenter != null)
                    {
                        dto.OperationName = workCenter.OperationName;

                        var joBOM = await _joBOMService.GetByIdAsync(dto.JobOpId);
                        if (joBOM != null)
                        {
                            dto.PartId = joBOM.PartId;
                        }
                    }
                }
            }

            return jobOperationStatusDTOs;
        }

        public async Task<List<JobOperationStatusViewLatest>> GetViewDataAsync()
        {
            return await _context.JobOperationStatusViewLatest.ToListAsync();
        }
    }
}
