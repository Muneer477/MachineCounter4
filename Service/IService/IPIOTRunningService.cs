using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IPIOTRunningService
    {
        Task<IEnumerable<PIOTRunningDTO>> GetAllAsync();
        Task<PIOTRunningDTO?> GetByIdAsync(int id);
        Task<PIOTRunningDTO?> CreateAsync(PIOTRunningDTO piotRunningDTO);
        Task<PIOTRunningDTO?> UpdateAsync(PIOTRunningDTO piotRunningDTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}