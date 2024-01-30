using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service.IService;
using SMTS.Utility;

namespace SMTS.Services
{
    public class PIOTMaintainanceService : IPIOTMaintainanceService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PIOTMaintainanceService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<PIOTMaintainanceDTO>> GetAllAsync()
        {
            var piotMaintainances = await _context.PIOTMaintenance.ToListAsync();
            return _mapper.Map<IEnumerable<PIOTMaintainanceDTO>>(piotMaintainances);
        }

        public async Task<PIOTMaintainanceDTO?> GetByIdAsync(int id)
        {
            var piotMaintainance = await _context.PIOTMaintenance.FindAsync(id);
            return _mapper.Map<PIOTMaintainanceDTO>(piotMaintainance);
        }

        public async Task<PIOTMaintainanceDTO?> CreateAsync(PIOTMaintainanceDTO piotMaintainanceDTO)
        {
            var piotMaintainance = _mapper.Map<PIOTMaintenance>(piotMaintainanceDTO);
            _context.PIOTMaintenance.Add(piotMaintainance);
            await _context.SaveChangesAsync();
            return _mapper.Map<PIOTMaintainanceDTO>(piotMaintainance);
        }

        public async Task<PIOTMaintainanceDTO> UpdateAsync(PIOTMaintainanceDTO piotMaintainanceDTO)
        {
            try
            {
                var piotMaintainance = await _context.PIOTMaintenance.FindAsync(piotMaintainanceDTO.Id);
                if (piotMaintainance == null)
                {
                    // You need to decide what to do if the piotMaintainance isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("PIOTMaintainance not found.");
                }

                _mapper.Map(piotMaintainanceDTO, piotMaintainance);
                _context.PIOTMaintenance.Update(piotMaintainance);
                await _context.SaveChangesAsync();
                return _mapper.Map<PIOTMaintainanceDTO>(piotMaintainance); // Map and return the updated piotMaintainance
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
            var piotMaintainance = await _context.PIOTMaintenance.FindAsync(id);
            if (piotMaintainance == null) return false;

            _context.PIOTMaintenance.Remove(piotMaintainance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {   
            int count = await _context.PIOTMaintenance.CountAsync();
            return count;
        }
    }
}
