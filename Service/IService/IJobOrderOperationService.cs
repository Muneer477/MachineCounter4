using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IJobOrderOperationService
    {
        Task<IEnumerable<JobOrderOperationDTO>> GetAllAsync();
        Task<JobOrderOperationDTO?> GetByIdAsync(int id);
        Task<JobOrderOperationDTO?> CreateAsync(JobOrderOperationDTO jobOrderOperationDto);
        Task<JobOrderOperationDTO?> UpdateAsync(JobOrderOperationDTO jobOrderOperationDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
