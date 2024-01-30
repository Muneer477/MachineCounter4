using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IPartUOMService
    {
        Task<IEnumerable<PartUOMDto>> GetAllAsync();
        Task<PartUOMDto?> GetByIdAsync(int id);
        Task<PartUOMDto?> CreateAsync(PartUOMDto partUOMDto);
        Task<PartUOMDto?> UpdateAsync(PartUOMDto partUOMDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
