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
    public class DepartmentService : IDepartmentService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _context.Department.ToListAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _context.Department.FindAsync(id);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto?> CreateAsync(DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Departments>(departmentDto);
            _context.Department.Add(department);
            await _context.SaveChangesAsync();
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> UpdateAsync(DepartmentDto departmentDto)
        {
            try
            {
                var department = await _context.Department.FindAsync(departmentDto.Id);
                if (department == null)
                {
                    // You need to decide what to do if the department isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("Department not found.");
                }

                _mapper.Map(departmentDto, department);
                _context.Department.Update(department);
                await _context.SaveChangesAsync();
                return _mapper.Map<DepartmentDto>(department); // Map and return the updated department
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
            var department = await _context.Department.FindAsync(id);
            if (department == null) return false;

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.Department.CountAsync();
            return count;
        }
    }
}
