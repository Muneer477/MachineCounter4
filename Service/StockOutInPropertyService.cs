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
    public class StockOutInPropertyService : IStockOutInPropertyService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public StockOutInPropertyService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<StockOutInPropertyDTO>> GetAllAsync()
        {
            var stockOutInPropertys = await _context.StockOutInProperty.ToListAsync();
            return _mapper.Map<IEnumerable<StockOutInPropertyDTO>>(stockOutInPropertys);
        }

        public async Task<StockOutInPropertyDTO?> GetByIdAsync(int id)
        {
            var stockOutInProperty = await _context.StockOutInProperty.FindAsync(id);
            return _mapper.Map<StockOutInPropertyDTO>(stockOutInProperty);
        }

        public async Task<StockOutInPropertyDTO?> CreateAsync(StockOutInPropertyDTO stockOutInPropertyDTO)
        {
            var stockOutInProperty = _mapper.Map<StockOutInProperty>(stockOutInPropertyDTO);
            _context.StockOutInProperty.Add(stockOutInProperty);
            await _context.SaveChangesAsync();
            return _mapper.Map<StockOutInPropertyDTO>(stockOutInProperty);
        }

        public async Task<StockOutInPropertyDTO> UpdateAsync(StockOutInPropertyDTO stockOutInPropertyDTO)
        {
            try
            {
                var stockOutInProperty = await _context.StockOutInProperty.FindAsync(stockOutInPropertyDTO.Id);
                if (stockOutInProperty == null)
                {
                    // You need to decide what to do if the stockOutInProperty isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("StockOutInProperty not found.");
                }

                _mapper.Map(stockOutInPropertyDTO, stockOutInProperty);
                _context.StockOutInProperty.Update(stockOutInProperty);
                await _context.SaveChangesAsync();
                return _mapper.Map<StockOutInPropertyDTO>(stockOutInProperty); // Map and return the updated stockOutInProperty
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
            var stockOutInProperty = await _context.StockOutInProperty.FindAsync(id);
            if (stockOutInProperty == null) return false;

            _context.StockOutInProperty.Remove(stockOutInProperty);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.StockOutInProperty.CountAsync();
            return count;
        }

        public async Task CreateWithoutReturn(int CategoryId, string value, int StockOutInDtlKey)
        {
            StockOutInPropertyDTO stockOutInPropertyDTO = new StockOutInPropertyDTO();
            stockOutInPropertyDTO.Value = value;
            stockOutInPropertyDTO.CategoryId = CategoryId;
            stockOutInPropertyDTO.StockOutInDtlKey = StockOutInDtlKey;
 
            var stockOutInProperty = _mapper.Map<StockOutInProperty>(stockOutInPropertyDTO);

            _context.StockOutInProperty.Add(stockOutInProperty);

            await _context.SaveChangesAsync();

        }
    }
}
