using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IPIOTCounterService
    {
        Task<IEnumerable<PIOTCounterDTO>> GetAllAsync();
        Task<PIOTCounterDTO?> GetByIdAsync(int id);
        Task<PIOTCounterDTO?> CreateAsync(PIOTCounterDTO piotCounterDTO);
        Task<PIOTCounterDTO?> UpdateAsync(PIOTCounterDTO piotCounterDTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task CreateAsync(PIOTRunningDTO jobOperationStatusDTO);
        Task UpdateAsync(PIOTRunningDTO jobOperationStatusDTO);
    }
}
