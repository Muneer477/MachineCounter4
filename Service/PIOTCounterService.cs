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
    public class PIOTCounterService : IPIOTCounterService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PIOTCounterService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<PIOTCounterDTO>> GetAllAsync()
        {
            var piotCounters = await _context.PIOTCounter.ToListAsync();
            return _mapper.Map<IEnumerable<PIOTCounterDTO>>(piotCounters);
        }

        public async Task<PIOTCounterDTO?> GetByIdAsync(int id)
        {
            var piotCounter = await _context.PIOTCounter.FindAsync(id);
            return _mapper.Map<PIOTCounterDTO>(piotCounter);
        }

        public async Task<PIOTCounterDTO?> CreateAsync(PIOTCounterDTO piotCounterDTO)
        {
            var piotCounter = _mapper.Map<PIOT_Counter>(piotCounterDTO);
            _context.PIOTCounter.Add(piotCounter);
            await _context.SaveChangesAsync();
            return _mapper.Map<PIOTCounterDTO>(piotCounter);
        }

        public async Task<PIOTCounterDTO> UpdateAsync(PIOTCounterDTO piotCounterDTO)
        {
            try
            {
                var piotCounter = await _context.PIOTCounter.FindAsync(piotCounterDTO.Id);
                if (piotCounter == null)
                {
                    // You need to decide what to do if the piotCounter isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("PIOTCounter not found.");
                }

                _mapper.Map(piotCounterDTO, piotCounter);
                _context.PIOTCounter.Update(piotCounter);
                await _context.SaveChangesAsync();
                return _mapper.Map<PIOTCounterDTO>(piotCounter); // Map and return the updated piotCounter
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
            var piotCounter = await _context.PIOTCounter.FindAsync(id);
            if (piotCounter == null) return false;

            _context.PIOTCounter.Remove(piotCounter);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.PIOTCounter.CountAsync();
            return count;
        }

        public Task CreateAsync(PIOTRunningDTO jobOperationStatusDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PIOTRunningDTO jobOperationStatusDTO)
        {
            throw new NotImplementedException();
        }
    }
}
