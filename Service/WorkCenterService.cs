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
    public class WorkCenterService : IWorkCenterService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public WorkCenterService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkCenterDto>> GetAllAsync()
        {
            var workCenters = await _context.WorkCenter.ToListAsync();
            return _mapper.Map<IEnumerable<WorkCenterDto>>(workCenters);
        }

       
        public async Task<WorkCenterDto?> GetByIdAsync(int id)
        {
            var workCenter = await _context.WorkCenter.FindAsync(id);
            return _mapper.Map<WorkCenterDto>(workCenter);
        }

        public async Task<WorkCenterDto?> CreateAsync(WorkCenterDto WorkCenterDto)
        {
            var WorkCenter = _mapper.Map<WorkCenter>(WorkCenterDto);
            _context.WorkCenter.Add(WorkCenter);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkCenterDto>(WorkCenter);
        }

        public async Task<WorkCenterDto> UpdateAsync(WorkCenterDto WorkCenterDto)
        {
            try
            {
                var WorkCenter = await _context.WorkCenter.FindAsync(WorkCenterDto.Id);
                if (WorkCenter == null)
                {
                    // You need to decide what to do if the WorkCenter isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("WorkCenter not found.");
                }

                _mapper.Map(WorkCenterDto, WorkCenter);
                _context.WorkCenter.Update(WorkCenter);
                await _context.SaveChangesAsync();
                return _mapper.Map<WorkCenterDto>(WorkCenter); // Map and return the updated WorkCenter
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
            var WorkCenter = await _context.WorkCenter.FindAsync(id);
            if (WorkCenter == null) return false;

            _context.WorkCenter.Remove(WorkCenter);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.WorkCenter.CountAsync();
            return count;
        }
        
    }
}