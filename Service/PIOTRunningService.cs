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
    public class PIOTRunningService : IPIOTRunningService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PIOTRunningService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<PIOTRunningDTO>> GetAllAsync()
        {
            var piotRunnings = await _context.PIOTRunning.ToListAsync();
            return _mapper.Map<IEnumerable<PIOTRunningDTO>>(piotRunnings);
        }

        public async Task<PIOTRunningDTO?> GetByIdAsync(int id)
        {
            var piotRunning = await _context.PIOTRunning.FindAsync(id);
            return _mapper.Map<PIOTRunningDTO>(piotRunning);
        }

        public async Task<PIOTRunningDTO?> CreateAsync(PIOTRunningDTO piotRunningDTO)
        {
            var piotRunning = _mapper.Map<PIOTRunning>(piotRunningDTO);
            _context.PIOTRunning.Add(piotRunning);
            await _context.SaveChangesAsync();
            return _mapper.Map<PIOTRunningDTO>(piotRunning);
        }

        public async Task<PIOTRunningDTO> UpdateAsync(PIOTRunningDTO piotRunningDTO)
        {
            try
            {
                var piotRunning = await _context.PIOTRunning.FindAsync(piotRunningDTO.Id);
                if (piotRunning == null)
                {
                    // You need to decide what to do if the piotRunning isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("PIOTRunning not found.");
                }

                _mapper.Map(piotRunningDTO, piotRunning);
                _context.PIOTRunning.Update(piotRunning);
                await _context.SaveChangesAsync();
                return _mapper.Map<PIOTRunningDTO>(piotRunning); // Map and return the updated piotRunning
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
            var piotRunning = await _context.PIOTRunning.FindAsync(id);
            if (piotRunning == null) return false;

            _context.PIOTRunning.Remove(piotRunning);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.PIOTRunning.CountAsync();
            return count;
        }
    }
}