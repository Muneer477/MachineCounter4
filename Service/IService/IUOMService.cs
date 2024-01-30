using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{    
    public interface IUOMService
    {
        Task<IEnumerable<UOMsDto>> GetAllAsync();
        Task<UOMsDto?> GetByIdAsync(int id);
        Task<UOMsDto?> CreateAsync(UOMsDto UOMsDto);
        Task<UOMsDto?> UpdateAsync(UOMsDto UOMsDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
