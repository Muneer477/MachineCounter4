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
    public class JobOrderService : IJobOrderService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public JobOrderService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<JobOrderDto>> GetAllAsync()
        {
            var jobOrders = await _context.JobOrder.ToListAsync();
            return _mapper.Map<IEnumerable<JobOrderDto>>(jobOrders);
        }

        public async Task<JobOrderDto?> GetByIdAsync(int id)
        {
            var jobOrder = await _context.JobOrder.FindAsync(id);
            return _mapper.Map<JobOrderDto>(jobOrder);
        }

        public async Task<JobOrderDto?> CreateAsync(JobOrderDto jobOrderDto)
        {
            var jobOrder = _mapper.Map<JobOrder>(jobOrderDto);
            _context.JobOrder.Add(jobOrder);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobOrderDto>(jobOrder);
        }

        public async Task<JobOrderDto> UpdateAsync(JobOrderDto jobOrderDto)
        {
            try
            {
                var jobOrder = await _context.JobOrder.FindAsync(jobOrderDto.Id);
                if (jobOrder == null)
                {
                    // You need to decide what to do if the jobOrder isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("JobOrder not found.");
                }

                _mapper.Map(jobOrderDto, jobOrder);
                _context.JobOrder.Update(jobOrder);
                await _context.SaveChangesAsync();
                return _mapper.Map<JobOrderDto>(jobOrder); // Map and return the updated jobOrder
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
            var jobOrder = await _context.JobOrder.FindAsync(id);
            if (jobOrder == null) return false;

            _context.JobOrder.Remove(jobOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.JobOrder.CountAsync();
            return count;
        }

        public async Task<JobOrderDto> getJobOrderInfoByOperationId(int OperationId)
        {
            var JoBOMId = await _context.JobOrderOperation.Where(row => row.Id == OperationId).Select(row => row.JoBOMId).FirstOrDefaultAsync();
            var JobOrderId = await _context.JoBOM.Where(row => row.Id == JoBOMId).Select(row => row.JoId).FirstOrDefaultAsync();
            var JobOrder = await _context.JobOrder.Where(row => row.Id == JobOrderId).Select(row => new {Id = row.Id, JoNo = row.JoNo}).FirstOrDefaultAsync();

            


            return new JobOrderDto {Id= JobOrder.Id, JoNo= JobOrder.JoNo };
        }
    }
}
