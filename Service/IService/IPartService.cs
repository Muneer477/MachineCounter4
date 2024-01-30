using SMTS.Entities;

using SMTS.DTOs;

namespace SMTS.Service.IService
{    
    public interface IPartService
    {      
        Task<IEnumerable<PartDto>> GetAllAsync();
        Task<PartDto?> GetByIdAsync(int id);
        Task<PartDto?> CreateAsync(PartDto partDto);
        Task<PartDto?> UpdateAsync(PartDto partDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();       
    }
}
