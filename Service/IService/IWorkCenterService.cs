using SMTS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMTS.Service.IService
{
    public interface IWorkCenterService
    {
        Task<IEnumerable<WorkCenterDto>> GetAllAsync();
        Task<WorkCenterDto?> GetByIdAsync(int id);
        Task<WorkCenterDto?> CreateAsync(WorkCenterDto workCentreDto);
        Task<WorkCenterDto?> UpdateAsync(WorkCenterDto workCentreDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
       
    }
}
