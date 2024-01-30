using SMTS.DTOs;

namespace SMTS.Service.IService
{
    public interface IWeightReadingService
    {
        Task<WeightReadingsDto?> CreateAsync(WeightReadingsDto weightReadingsDto);
        Task<WeightReadingsDto?> GetByIdAsync(int id);
    }
}
