using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IPlanningJOService
    {
        Task<IEnumerable<PlanningJODTO>> GetAllAsync();
        Task<PlanningJODTO?> GetByIdAsync(int id);
        Task<PlanningJODTO?> CreateAsync(PlanningJODTO planningJODTO);
        Task<PlanningJODTO?> UpdateAsync(PlanningJODTO planningJODTO);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
