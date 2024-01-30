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
    public class PartUOMService : IPartUOMService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PartUOMService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartUOMDto>> GetAllAsync()
        {
            var partUOMs = await _context.PartUOM.ToListAsync();
            return _mapper.Map<IEnumerable<PartUOMDto>>(partUOMs);
        }

        public async Task<PartUOMDto?> GetByIdAsync(int id)
        {
            var partUOM = await _context.PartUOM.FindAsync(id);
            return _mapper.Map<PartUOMDto>(partUOM);
        }
              

        public async Task<bool> InsertPartUOM(PartUOMDto partUOMDto)
        {
            // Check if the PartId exists in the PartDTL table
            bool partExists = await _context.PartUOM.AnyAsync(p => p.Id == partUOMDto.PartId);
            if (!partExists)
            {
                // Handle the case where PartId doesn't exist
                // You can log this issue, throw an exception, or return a specific result
                return false;
            }

            var partUOM = _mapper.Map<PartUOM>(partUOMDto);
            _context.PartUOM.Add(partUOM);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PartUOMDto?> CreateAsync(PartUOMDto partUOMDto)
        {
            var partUOM = _mapper.Map<PartUOM>(partUOMDto);
            _context.PartUOM.Add(partUOM);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartUOMDto>(partUOM);
        }       

        public async Task<PartUOMDto> UpdateAsync(PartUOMDto partUOMDto)
        {
            try
            {
                var partUOM = await _context.PartUOM.FindAsync(partUOMDto.Id);
                if (partUOM == null)
                {
                    // You need to decide what to do if the partUOM isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("PartUOM not found.");
                }

                _mapper.Map(partUOMDto, partUOM);
                _context.PartUOM.Update(partUOM);
                await _context.SaveChangesAsync();
                return _mapper.Map<PartUOMDto>(partUOM); // Map and return the updated partUOM
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
            var partUOM = await _context.PartUOM.FindAsync(id);
            if (partUOM == null) return false;

            _context.PartUOM.Remove(partUOM);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.PartUOM.CountAsync();
            return count;
        }
    }
}
