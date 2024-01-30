using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs.Stock;
using SMTS.Entities;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMTS.Utility;
using SMTS.DTOs;

namespace SMTS.Services
{
    public class StockOutInDTLService : IStockOutInDTLService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public StockOutInDTLService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<StockOutInDTLDTO>> GetAllAsync()
        {
            var stockOutInDTLs = await _context.StockOutInDTL.ToListAsync();
            return _mapper.Map<IEnumerable<StockOutInDTLDTO>>(stockOutInDTLs);
        }

        public async Task<StockOutInDTLDTO?> GetByIdAsync(int id)
        {
            var stockOutInDTL = await _context.StockOutInDTL.FindAsync(id);
            return _mapper.Map<StockOutInDTLDTO>(stockOutInDTL);
        }

        public async Task<StockOutInDTLDTO?> CreateAsync(StockOutInDTLDTO stockOutInDTLDto)
        {
            var stockOutInDTL = _mapper.Map<StockOutInDTL>(stockOutInDTLDto);
            _context.StockOutInDTL.Add(stockOutInDTL);
            await _context.SaveChangesAsync();
            return _mapper.Map<StockOutInDTLDTO>(stockOutInDTL);
        }

        public async Task<StockOutInDTLDTO> UpdateAsync(StockOutInDTLDTO stockOutInDTLDto)
        {
            try
            {
                var stockOutInDTL = await _context.StockOutInDTL.FindAsync(stockOutInDTLDto.DtlKey);
                if (stockOutInDTL == null)
                {
                    // You need to decide what to do if the stockOutInDTL isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("StockOutInDTL not found.");
                }

                _mapper.Map(stockOutInDTLDto, stockOutInDTL);
                _context.StockOutInDTL.Update(stockOutInDTL);
                await _context.SaveChangesAsync();
                return _mapper.Map<StockOutInDTLDTO>(stockOutInDTL); // Map and return the updated stockOutInDTL
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
            var stockOutInDTL = await _context.StockOutInDTL.FindAsync(id);
            if (stockOutInDTL == null) return false;

            _context.StockOutInDTL.Remove(stockOutInDTL);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.StockOutInDTL.CountAsync();
            return count;
        }

        public async Task<IEnumerable<StockOutInDTLDTO>> GetAllAsyncStockOutInDTLPartDTL() //GetAllAsync from StockOutInDTL left join PartDTL
        {
            // Get the list of StockOutInDTL entities from the database
            var stockOutInDTLs = await _context.StockOutInDTL.ToListAsync();

            // Map the StockOutInDTL entities to StockOutInDTLDto
            var stockOutInDTLDtos = _mapper.Map<IEnumerable<StockOutInDTLDTO>>(stockOutInDTLs);

            // Create a dictionary to store PartId-PartCode-Description mappings for efficient lookup
            var partIdToPartInfo = new Dictionary<int?, (string?, string?)>();

            // Fetch the PartCode and Description for each unique PartId and store them in the dictionary
            var uniquePartIds = stockOutInDTLDtos.Select(dto => dto.PartId).Distinct().ToList();
            foreach (var partId in uniquePartIds)
            {
                var partInfo = await _context.PartDTL
                    .Where(part => part.Id == partId)
                    .Select(part => new { part.PartCode, part.Description })
                    .FirstOrDefaultAsync();

                if (partInfo != null)
                {
                    partIdToPartInfo[partId] = (partInfo.PartCode, partInfo.Description);
                }
            }

            // Enrich the StockOutInDTLDtos with PartCode and Description based on PartId
            foreach (var dto in stockOutInDTLDtos)
            {
                if (partIdToPartInfo.TryGetValue(dto.PartId, out var partInfo))
                {
                    dto.PartCode = partInfo.Item1;
                    dto.Description = partInfo.Item2;
                }
            }

            return stockOutInDTLDtos;
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
