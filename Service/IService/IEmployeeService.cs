using SMTS.Entities;
using SMTS.DTOs;

namespace SMTS.Service.IService
{    
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto?> CreateAsync(EmployeeDto employeeDto);
        Task<EmployeeDto?> UpdateAsync(EmployeeDto employeeDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
