using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;

namespace EmployeeDirectory.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();

            return departments.Select(MapToDto);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);

            return department is null
                ? null
                : MapToDto(department);
        }

        public async Task<DepartmentDto> CreateAsync(
            CreateDepartmentDto departmentDto)
        {
            ArgumentNullException.ThrowIfNull(departmentDto);

            var department = new Department
            {
                Name = departmentDto.Name.Trim(),
                Description = departmentDto.Description.Trim()
            };

            var createdDepartment =
                await _departmentRepository.AddAsync(department);

            return MapToDto(createdDepartment);
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateDepartmentDto departmentDto)
        {
            ArgumentNullException.ThrowIfNull(departmentDto);

            var existingDepartment =
                await _departmentRepository.GetByIdAsync(id);

            if (existingDepartment is null)
            {
                return false;
            }

            existingDepartment.Name = departmentDto.Name.Trim();
            existingDepartment.Description =
                departmentDto.Description.Trim();

            await _departmentRepository.UpdateAsync(existingDepartment);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingDepartment =
                await _departmentRepository.GetByIdAsync(id);

            if (existingDepartment is null)
            {
                return false;
            }

            await _departmentRepository.DeleteAsync(id);

            return true;
        }

        private static DepartmentDto MapToDto(Department department)
        {
            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }
    }
}
