using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMTS.Utility;

namespace SMTS.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;       

        public EmployeeService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var Employees = await _context.Employee.ToListAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(Employees);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var Employee = await _context.Employee.FindAsync(id);
            return _mapper.Map<EmployeeDto>(Employee);
        }

        public async Task<EmployeeDto?> CreateAsync(EmployeeDto EmployeeDto)
        {
            var Employee = _mapper.Map<Employee>(EmployeeDto);
            _context.Employee.Add(Employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeDto>(Employee);
        }

        public async Task<EmployeeDto> UpdateAsync(EmployeeDto EmployeeDto)
        {
            try
            {
                var Employee = await _context.Employee.FindAsync(EmployeeDto.Id);
                if (Employee == null)
                {
                    // You need to decide what to do if the Employee isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("Employee not found.");
                }

                _mapper.Map(EmployeeDto, Employee);
                _context.Employee.Update(Employee);
                await _context.SaveChangesAsync();
                return _mapper.Map<EmployeeDto>(Employee); // Map and return the updated Employee
            }
            catch (DbUpdateException ex)
            {
                if (IsForeignKeyViolation(ex))
                {
                    // Handle the foreign key violation
                    throw new CustomException("Foreign key constraint violated.");
                }
                else
                {
                    // Handle other types of DbUpdateException or rethrow
                    // Depending on your use case, you might want to return a default value, null, or throw
                    throw; // Rethrows the current exception
                }
            }
            // If there are other potential exceptions that should be caught and handled differently,
            // add additional catch blocks here
        }        

        private bool IsForeignKeyViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as SqlException;

            if (sqlException != null)
            {
                foreach (SqlError error in sqlException.Errors)
                {
                    // In SQL Server, the number for a foreign key violation is 547
                    if (error.Number == 547)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Employee = await _context.Employee.FindAsync(id);
            if (Employee == null) return false;

            _context.Employee.Remove(Employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.Employee.CountAsync();
            return count;
        }
    }
}
