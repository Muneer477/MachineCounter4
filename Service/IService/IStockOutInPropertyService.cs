using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IStockOutInPropertyService
    {
        Task<IEnumerable<StockOutInPropertyDTO>> GetAllAsync();
        Task<StockOutInPropertyDTO?> GetByIdAsync(int id);
        Task<StockOutInPropertyDTO?> CreateAsync(StockOutInPropertyDTO departmentDto);
        Task<StockOutInPropertyDTO?> UpdateAsync(StockOutInPropertyDTO departmentDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task CreateWithoutReturn(int CategoryId, string value, int StockOutInDtlKey);
    }
}
