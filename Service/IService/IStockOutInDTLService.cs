
using SMTS.DTOs;
using SMTS.DTOs.Stock;

namespace SMTS.Service.IService
{    
    public interface IStockOutInDTLService
    {
        Task<IEnumerable<StockOutInDTLDTO>> GetAllAsync();
        Task<StockOutInDTLDTO?> GetByIdAsync(int id);
        Task<StockOutInDTLDTO?> CreateAsync(StockOutInDTLDTO stockOutInDTLDto);
        Task<StockOutInDTLDTO?> UpdateAsync(StockOutInDTLDTO stockOutInDTLDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<IEnumerable<StockOutInDTLDTO>> GetAllAsyncStockOutInDTLPartDTL();
        Task<BlowingRollSuffixDTO> getRollSuffixAsync(string Jono);

    }
}
