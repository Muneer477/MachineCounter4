using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IJoBOMService
    {
        Task<IEnumerable<JoBOMDTO>> GetAllAsync();
        Task<JoBOMDTO?> GetByIdAsync(int id);
        Task<JoBOMDTO?> CreateAsync(JoBOMDTO joBOMDTO);
        Task<JoBOMDTO?> UpdateAsync(JoBOMDTO joBOMDTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<int> GetPartIdByJobOperationId(int OperationId);
    }
}
