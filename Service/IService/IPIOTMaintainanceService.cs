using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IPIOTMaintainanceService
    {
        Task<IEnumerable<PIOTMaintainanceDTO>> GetAllAsync();
        Task<PIOTMaintainanceDTO?> GetByIdAsync(int id);
        Task<PIOTMaintainanceDTO?> CreateAsync(PIOTMaintainanceDTO piotMaintainanceDTO);
        Task<PIOTMaintainanceDTO?> UpdateAsync(PIOTMaintainanceDTO piotMaintainanceDTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
