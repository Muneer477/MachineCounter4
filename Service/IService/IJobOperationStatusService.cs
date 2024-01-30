using SMTS.Entities;
using SMTS.DTOs;
using SMTS.DTOs.Stock;

namespace SMTS.Service.IService
{
    public interface IJobOperationStatusService
    {
        Task<IEnumerable<JobOperationStatusDTO>> GetAllAsync();
        Task<JobOperationStatusDTO?> GetByIdAsync(int id);
        Task<JobOperationStatusDTO?> CreateAsync(JobOperationStatusDTO jobOperationStatusDto);
        Task<JobOperationStatusDTO?> UpdateAsync(JobOperationStatusDTO jobOperationStatusDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<List<JobOrderWithOperationStatusDTO>> GetByJoStatusOperationAsync(string status, string Operation);
        Task<List<JobOperationStatusViewLatest>> GetViewDataAsync();        


    }
}
