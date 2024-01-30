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
    public class PlanningJOService : IPlanningJOService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public PlanningJOService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<PlanningJODTO>> GetAllAsync()
        {
            var planningJOs = await _context.PlanningJO.ToListAsync();
            return _mapper.Map<IEnumerable<PlanningJODTO>>(planningJOs);
        }

        public async Task<PlanningJODTO?> GetByIdAsync(int id)
        {
            var planningJO = await _context.PlanningJO.FindAsync(id);
            return _mapper.Map<PlanningJODTO>(planningJO);
        }

        public async Task<PlanningJODTO?> CreateAsync(PlanningJODTO planningJODTO)
        {
            var planningJO = _mapper.Map<PlanningJO>(planningJODTO);
            _context.PlanningJO.Add(planningJO);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlanningJODTO>(planningJO);
        }

        public async Task<PlanningJODTO> UpdateAsync(PlanningJODTO planningJODTO)
        {
            try
            {
                var planningJO = await _context.PlanningJO.FindAsync(planningJODTO.Id);
                if (planningJO == null)
                {
                    // You need to decide what to do if the planningJO isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("PlanningJO not found.");
                }

                _mapper.Map(planningJODTO, planningJO);
                _context.PlanningJO.Update(planningJO);
                await _context.SaveChangesAsync();
                return _mapper.Map<PlanningJODTO>(planningJO); // Map and return the updated planningJO
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
            var planningJO = await _context.PlanningJO.FindAsync(id);
            if (planningJO == null) return false;

            _context.PlanningJO.Remove(planningJO);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.PlanningJO.CountAsync();
            return count;
        }
    }
}
