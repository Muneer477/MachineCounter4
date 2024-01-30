using SMTS.Entities;

using SMTS.DTOs;
using SMTS.DTOs.Stock;

namespace SMTS.Service.IService
{    
    public interface IJobOrderService
    {
        Task<IEnumerable<JobOrderDto>> GetAllAsync();
        Task<JobOrderDto?> GetByIdAsync(int id);
        Task<JobOrderDto?> CreateAsync(JobOrderDto jobOrderDto);
        Task<JobOrderDto?> UpdateAsync(JobOrderDto jobOrderDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<JobOrderDto> getJobOrderInfoByOperationId(int OperationId);
    }
}
