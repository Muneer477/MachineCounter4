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
    public class PartDTLPrefixBatchSNService : IPartDTLPrefixBatchSNService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PartDTLPrefixBatchSNService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<PartDTLPrefixBatchSNDto>> GetAllAsync()
        {
            var partDTLPrefixBatchSNs = await _context.PartDTLPrefixBatchSN.ToListAsync();
            return _mapper.Map<IEnumerable<PartDTLPrefixBatchSNDto>>(partDTLPrefixBatchSNs);
        }

        public async Task<PartDTLPrefixBatchSNDto?> GetByIdAsync(int id)
        {
            var partDTLPrefixBatchSN = await _context.PartDTLPrefixBatchSN.FindAsync(id);
            return _mapper.Map<PartDTLPrefixBatchSNDto>(partDTLPrefixBatchSN);
        }

        public async Task<PartDTLPrefixBatchSNDto?> CreateAsync(PartDTLPrefixBatchSNDto partDTLPrefixBatchSNDto)
        {
            var partDTLPrefixBatchSN = _mapper.Map<PartDTLPrefixBatchSN>(partDTLPrefixBatchSNDto);
            _context.PartDTLPrefixBatchSN.Add(partDTLPrefixBatchSN);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartDTLPrefixBatchSNDto>(partDTLPrefixBatchSN);
        }

        public async Task<PartDTLPrefixBatchSNDto> UpdateAsync(PartDTLPrefixBatchSNDto partDTLPrefixBatchSNDto)
        {
            try
            {
                var partDTLPrefixBatchSN = await _context.PartDTLPrefixBatchSN.FindAsync(partDTLPrefixBatchSNDto.Id);
                if (partDTLPrefixBatchSN == null)
                {
                    // You need to decide what to do if the partDTLPrefixBatchSN isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("PartDTLPrefixBatchSN not found.");
                }

                _mapper.Map(partDTLPrefixBatchSNDto, partDTLPrefixBatchSN);
                _context.PartDTLPrefixBatchSN.Update(partDTLPrefixBatchSN);
                await _context.SaveChangesAsync();
                return _mapper.Map<PartDTLPrefixBatchSNDto>(partDTLPrefixBatchSN); // Map and return the updated partDTLPrefixBatchSN
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
            var partDTLPrefixBatchSN = await _context.PartDTLPrefixBatchSN.FindAsync(id);
            if (partDTLPrefixBatchSN == null) return false;

            _context.PartDTLPrefixBatchSN.Remove(partDTLPrefixBatchSN);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.PartDTLPrefixBatchSN.CountAsync();
            return count;
        }
    }
}
