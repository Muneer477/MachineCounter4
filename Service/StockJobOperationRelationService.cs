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
    public class StockJobOperationRelationService : IStockJobOperationRelationService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public StockJobOperationRelationService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<StockJobOperationRelationDTO>> GetAllAsync()
        {
            var stockJobOperationRelations = await _context.StockJobOperationRelation.ToListAsync();
            return _mapper.Map<IEnumerable<StockJobOperationRelationDTO>>(stockJobOperationRelations);
        }

        public async Task<StockJobOperationRelationDTO?> GetByIdAsync(int id)
        {
            var stockJobOperationRelation = await _context.StockJobOperationRelation.FindAsync(id);
            return _mapper.Map<StockJobOperationRelationDTO>(stockJobOperationRelation);
        }

        public async Task<StockJobOperationRelationDTO?> CreateAsync(StockJobOperationRelationDTO stockJobOperationRelationDTO)
        {
            var stockJobOperationRelation = _mapper.Map<StockJobOperationRelation>(stockJobOperationRelationDTO);
            _context.StockJobOperationRelation.Add(stockJobOperationRelation);
            await _context.SaveChangesAsync();
            return _mapper.Map<StockJobOperationRelationDTO>(stockJobOperationRelation);
        }

        public async Task<StockJobOperationRelationDTO> UpdateAsync(StockJobOperationRelationDTO stockJobOperationRelationDTO)
        {
            try
            {
                var stockJobOperationRelation = await _context.StockJobOperationRelation.FindAsync(stockJobOperationRelationDTO.JobOperationId);
                if (stockJobOperationRelation == null)
                {
                    // You need to decide what to do if the stockJobOperationRelation isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("StockJobOperationRelation not found.");
                }

                _mapper.Map(stockJobOperationRelationDTO, stockJobOperationRelation);
                _context.StockJobOperationRelation.Update(stockJobOperationRelation);
                await _context.SaveChangesAsync();
                return _mapper.Map<StockJobOperationRelationDTO>(stockJobOperationRelation); // Map and return the updated stockJobOperationRelation
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
            var stockJobOperationRelation = await _context.StockJobOperationRelation.FindAsync(id);
            if (stockJobOperationRelation == null) return false;

            _context.StockJobOperationRelation.Remove(stockJobOperationRelation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.StockJobOperationRelation.CountAsync();
            return count;
        }

        public async Task CreateAsyncWithOutReturn(int JobOperationId, int DtlKey)
        {
            StockJobOperationRelationDTO stockJobOperationRelationDTO = new StockJobOperationRelationDTO()
            {
                JobOperationId = JobOperationId,
                StockDtlKey = DtlKey
            };
            var Relation = _mapper.Map<StockJobOperationRelation>(stockJobOperationRelationDTO);

            _context.StockJobOperationRelation.Add(Relation);

            await _context.SaveChangesAsync();
         }
    }
}
