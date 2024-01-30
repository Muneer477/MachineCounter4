using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IPartDTLPrefixBatchSNService
    {
        Task<IEnumerable<PartDTLPrefixBatchSNDto>> GetAllAsync();
        Task<PartDTLPrefixBatchSNDto?> GetByIdAsync(int id);
        Task<PartDTLPrefixBatchSNDto?> CreateAsync(PartDTLPrefixBatchSNDto partDTLPrefixBatchSNDto);
        Task<PartDTLPrefixBatchSNDto?> UpdateAsync(PartDTLPrefixBatchSNDto partDTLPrefixBatchSNDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
