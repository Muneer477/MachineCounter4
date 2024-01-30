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
    public class JoBOMService : IJoBOMService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public JoBOMService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<JoBOMDTO>> GetAllAsync()
        {
            var joBOMs = await _context.JoBOM.ToListAsync();
            return _mapper.Map<IEnumerable<JoBOMDTO>>(joBOMs);
        }

        public async Task<JoBOMDTO?> GetByIdAsync(int id)
        {
            var joBOM = await _context.JoBOM.FindAsync(id);
            return _mapper.Map<JoBOMDTO>(joBOM);
        }

        public async Task<JoBOMDTO?> CreateAsync(JoBOMDTO joBOMDTO)
        {
            var joBOM = _mapper.Map<JoBOM>(joBOMDTO);
            _context.JoBOM.Add(joBOM);
            await _context.SaveChangesAsync();
            return _mapper.Map<JoBOMDTO>(joBOM);
        }

        public async Task<JoBOMDTO> UpdateAsync(JoBOMDTO joBOMDTO)
        {
            try
            {
                var joBOM = await _context.JoBOM.FindAsync(joBOMDTO.Id);
                if (joBOM == null)
                {
                    // You need to decide what to do if the joBOM isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("JoBOM not found.");
                }

                _mapper.Map(joBOMDTO, joBOM);
                _context.JoBOM.Update(joBOM);
                await _context.SaveChangesAsync();
                return _mapper.Map<JoBOMDTO>(joBOM); // Map and return the updated joBOM
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
            var joBOM = await _context.JoBOM.FindAsync(id);
            if (joBOM == null) return false;

            _context.JoBOM.Remove(joBOM);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.JoBOM.CountAsync();
            return count;
        }

        public async Task<int> GetPartIdByJobOperationId(int OperationId)
        {
            var JoBOMId = await _context.JobOrderOperation.Where(row => row.Id == OperationId).Select(row => row.JoBOMId).FirstOrDefaultAsync();
            var PartId = await _context.JoBOM.Where(row => row.Id == JoBOMId).Select(row => row.PartId).FirstOrDefaultAsync();
            return PartId;
        }
    }
}
