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

using SMTS.DTOs.Stock;

namespace SMTS.Services
{
    public class StockOutInService : IStockOutInService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public StockOutInService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<StockOutInDTO>> GetAllAsync()
        {
            var stockOutIns = await _context.StockOutIn.ToListAsync();
            return _mapper.Map<IEnumerable<StockOutInDTO>>(stockOutIns);
        }

        public async Task<StockOutInDTO?> GetByIdAsync(int id)
        {
            var stockOutIn = await _context.StockOutIn.FindAsync(id);
            return _mapper.Map<StockOutInDTO>(stockOutIn);
        }

        public async Task<StockOutInDTO?> CreateAsync(StockOutInDTO stockOutInDTO)
        {
            var stockOutIn = _mapper.Map<StockOutIn>(stockOutInDTO);
            _context.StockOutIn.Add(stockOutIn);
            await _context.SaveChangesAsync();
            return _mapper.Map<StockOutInDTO>(stockOutIn);
        }

        public async Task<StockOutInDTO> UpdateAsync(StockOutInDTO stockOutInDTO)
        {
            try
            {
                var stockOutIn = await _context.StockOutIn.FindAsync(stockOutInDTO.Id);
                if (stockOutIn == null)
                {
                    // You need to decide what to do if the stockOutIn isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("StockOutIn not found.");
                }

                _mapper.Map(stockOutInDTO, stockOutIn);
                _context.StockOutIn.Update(stockOutIn);
                await _context.SaveChangesAsync();
                return _mapper.Map<StockOutInDTO>(stockOutIn); // Map and return the updated stockOutIn
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
            var stockOutIn = await _context.StockOutIn.FindAsync(id);
            if (stockOutIn == null) return false;

            _context.StockOutIn.Remove(stockOutIn);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.StockOutIn.CountAsync();
            return count;
        }

        public async Task<BlowingRollSuffixDTO> getRollSuffixAsync(string Jono)
        {
            var result = await _context.StockOutInDTL
                .Where(s => s.SerialNumber.StartsWith(Jono))
                .OrderByDescending(s => s.SerialNumber)
                .Take(1)
                .Select(s => new
                {
                    s.SerialNumber,
                    Prefix = s.SerialNumber.Substring(0, s.SerialNumber.IndexOf('-')),
                    Suffix = s.SerialNumber.Substring(s.SerialNumber.IndexOf('-') + 1),
                    SuffixPlusOne = "T" + (int.Parse(s.SerialNumber.Substring(s.SerialNumber.IndexOf('-') + 2)) + 1).ToString("D2")
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return new BlowingRollSuffixDTO()
                {
                    SuffixPlusOne = "T01"
                };
            }




            return new BlowingRollSuffixDTO() { SerialNumber = result.SerialNumber, Suffix = result.Suffix, Prefix = result.Prefix, SuffixPlusOne = result.SuffixPlusOne };


        }
    }
}
