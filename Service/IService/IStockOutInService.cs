using SMTS.Entities;
using SMTS.DTOs;
using SMTS.DTOs.Stock;

namespace SMTS.Service.IService
{
    public interface IStockOutInService
    {
        Task<IEnumerable<StockOutInDTO>> GetAllAsync();
        Task<StockOutInDTO?> GetByIdAsync(int id);
        Task<StockOutInDTO?> CreateAsync(StockOutInDTO stockOutInDTO);
        Task<StockOutInDTO?> UpdateAsync(StockOutInDTO stockOutInDTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
