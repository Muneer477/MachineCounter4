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
    public class PartService : IPartService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PartService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<PartDto>> GetAllAsync()
        {
            var parts = await _context.Part.ToListAsync();
            return _mapper.Map<IEnumerable<PartDto>>(parts);
        }

        public async Task<PartDto?> GetByIdAsync(int id)
        {
            var part = await _context.Part.FindAsync(id);
            return _mapper.Map<PartDto>(part);
        }

        public async Task<PartDto?> CreateAsync(PartDto partDto)
        {
            var part = _mapper.Map<PartDTL>(partDto);
            _context.Part.Add(part);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartDto>(part);
        }

        public async Task<PartDto> UpdateAsync(PartDto partDto)
        {
            try
            {
                var part = await _context.Part.FindAsync(partDto.Id);
                if (part == null)
                {
                    // You need to decide what to do if the part isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("Part not found.");
                }

                _mapper.Map(partDto, part);
                _context.Part.Update(part);
                await _context.SaveChangesAsync();
                return _mapper.Map<PartDto>(part); // Map and return the updated part
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
            var part = await _context.Part.FindAsync(id);
            if (part == null) return false;

            _context.Part.Remove(part);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.Part.CountAsync();
            return count;
        }
    }
}
