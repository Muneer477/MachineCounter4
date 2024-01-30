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

namespace SMTS.Services
{
    public class JobOrderOperationService : IJobOrderOperationService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public JobOrderOperationService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<JobOrderOperationDTO>> GetAllAsync()
        {
            var jobOrderOperations = await _context.JobOrderOperation.ToListAsync();
            return _mapper.Map<IEnumerable<JobOrderOperationDTO>>(jobOrderOperations);
        }

        public async Task<JobOrderOperationDTO?> GetByIdAsync(int id)
        {
            var jobOrderOperation = await _context.JobOrderOperation.FindAsync(id);
            return _mapper.Map<JobOrderOperationDTO>(jobOrderOperation);
        }

        public async Task<JobOrderOperationDTO?> CreateAsync(JobOrderOperationDTO jobOrderOperationDTO)
        {
            var jobOrderOperation = _mapper.Map<JobOrderOperation>(jobOrderOperationDTO);
            _context.JobOrderOperation.Add(jobOrderOperation);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobOrderOperationDTO>(jobOrderOperation);
        }

        public async Task<JobOrderOperationDTO> UpdateAsync(JobOrderOperationDTO jobOrderOperationDTO)
        {
            try
            {
                var jobOrderOperation = await _context.JobOrderOperation.FindAsync(jobOrderOperationDTO.Id);
                if (jobOrderOperation == null)
                {
                    // You need to decide what to do if the jobOrderOperation isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("JobOrderOperation not found.");
                }

                _mapper.Map(jobOrderOperationDTO, jobOrderOperation);
                _context.JobOrderOperation.Update(jobOrderOperation);
                await _context.SaveChangesAsync();
                return _mapper.Map<JobOrderOperationDTO>(jobOrderOperation); // Map and return the updated jobOrderOperation
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
            var jobOrderOperation = await _context.JobOrderOperation.FindAsync(id);
            if (jobOrderOperation == null) return false;

            _context.JobOrderOperation.Remove(jobOrderOperation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.JobOrderOperation.CountAsync();
            return count;
        }
    }
}
