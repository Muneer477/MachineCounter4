using SMTS.Entities;

using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IStockJobOperationRelationService
    {
        Task<IEnumerable<StockJobOperationRelationDTO>> GetAllAsync();
        Task<StockJobOperationRelationDTO?> GetByIdAsync(int id);
        Task<StockJobOperationRelationDTO?> CreateAsync(StockJobOperationRelationDTO stockJobOperationRelationDTO);
        Task<StockJobOperationRelationDTO?> UpdateAsync(StockJobOperationRelationDTO stockJobOperationRelationDTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task CreateAsyncWithOutReturn(int JobOperationId, int DtlKey);
    }
}
