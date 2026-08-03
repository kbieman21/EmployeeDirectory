using AutoMapper;
using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        //{
        //    var employees = await _employeeRepository.GetAllAsync();

        //    return employees.Select(MapToDto);
        //}

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        //public async Task<EmployeeDto?> GetByIdAsync(int id)
        //{
        //    var employee = await _employeeRepository.GetByIdAsync(id);

        //    return employee is null
        //        ? null
        //        : MapToDto(employee);
        //}

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            return employee is null
                ? null
                : _mapper.Map<EmployeeDto>(employee);
        }

        //public async Task<EmployeeDto> CreateAsync(
        //    CreateEmployeeDto employeeDto)
        //{
        //    ArgumentNullException.ThrowIfNull(employeeDto);

        //    var department =
        //        await _departmentRepository.GetByIdAsync(employeeDto.DepartmentId);

        //    if (department is null)
        //    {
        //        throw new ArgumentException(
        //            $"Department with ID {employeeDto.DepartmentId} does not exist.");
        //    }

        //    var emailExists =
        //        await _employeeRepository.EmailExistsAsync(employeeDto.Email);

        //    if (emailExists)
        //    {
        //        throw new InvalidOperationException(
        //            $"An employee with email '{employeeDto.Email}' already exists.");
        //    }

        //    var employee = new Employee
        //    {
        //        FirstName = employeeDto.FirstName.Trim(),
        //        LastName = employeeDto.LastName.Trim(),
        //        Email = employeeDto.Email.Trim(),
        //        JobTitle = employeeDto.JobTitle.Trim(),
        //        HireDate = employeeDto.HireDate,
        //        DepartmentId = employeeDto.DepartmentId
        //    };

        //    var createdEmployee =
        //        await _employeeRepository.AddAsync(employee);

        //    return MapToDto(createdEmployee);
        //}

        public async Task<EmployeeDto> CreateAsync(
    CreateEmployeeDto employeeDto)
        {
            ArgumentNullException.ThrowIfNull(employeeDto);

            var department =
                await _departmentRepository.GetByIdAsync(
                    employeeDto.DepartmentId);

            if (department is null)
            {
                throw new ArgumentException(
                    $"Department with ID {employeeDto.DepartmentId} does not exist.");
            }

            var emailExists =
                await _employeeRepository.EmailExistsAsync(
                    employeeDto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"An employee with email '{employeeDto.Email}' already exists.");
            }

            var employee = _mapper.Map<Employee>(employeeDto);

            employee.FirstName = employee.FirstName.Trim();
            employee.LastName = employee.LastName.Trim();
            employee.Email = employee.Email.Trim();
            employee.JobTitle = employee.JobTitle.Trim();

            var createdEmployee =
                await _employeeRepository.AddAsync(employee);

            return _mapper.Map<EmployeeDto>(createdEmployee);
        }


        //public async Task<bool> UpdateAsync(
        //    int id,
        //    UpdateEmployeeDto employeeDto)
        //{
        //    ArgumentNullException.ThrowIfNull(employeeDto);

        //    var existingEmployee =
        //        await _employeeRepository.GetByIdAsync(id);

        //    if (existingEmployee is null)
        //    {
        //        return false;
        //    }

        //    var department =
        //        await _departmentRepository.GetByIdAsync(employeeDto.DepartmentId);

        //    if (department is null)
        //    {
        //        throw new ArgumentException(
        //            $"Department with ID {employeeDto.DepartmentId} does not exist.");
        //    }

        //    var emailExists =
        //        await _employeeRepository.EmailExistsAsync(
        //            employeeDto.Email,
        //            id);

        //    if (emailExists)
        //    {
        //        throw new InvalidOperationException(
        //            $"Another employee with email '{employeeDto.Email}' already exists.");
        //    }

        //    existingEmployee.FirstName = employeeDto.FirstName.Trim();
        //    existingEmployee.LastName = employeeDto.LastName.Trim();
        //    existingEmployee.Email = employeeDto.Email.Trim();
        //    existingEmployee.JobTitle = employeeDto.JobTitle.Trim();
        //    existingEmployee.HireDate = employeeDto.HireDate;
        //    existingEmployee.DepartmentId = employeeDto.DepartmentId;

        //    await _employeeRepository.UpdateAsync(existingEmployee);

        //    return true;
        //}

        public async Task<bool> UpdateAsync(
    int id,
    UpdateEmployeeDto employeeDto)
        {
            ArgumentNullException.ThrowIfNull(employeeDto);

            var existingEmployee =
                await _employeeRepository.GetByIdAsync(id);

            if (existingEmployee is null)
            {
                return false;
            }

            var department =
                await _departmentRepository.GetByIdAsync(
                    employeeDto.DepartmentId);

            if (department is null)
            {
                throw new ArgumentException(
                    $"Department with ID {employeeDto.DepartmentId} does not exist.");
            }

            var emailExists =
                await _employeeRepository.EmailExistsAsync(
                    employeeDto.Email,
                    id);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"Another employee with email '{employeeDto.Email}' already exists.");
            }

            _mapper.Map(employeeDto, existingEmployee);

            existingEmployee.FirstName =
                existingEmployee.FirstName.Trim();

            existingEmployee.LastName =
                existingEmployee.LastName.Trim();

            existingEmployee.Email =
                existingEmployee.Email.Trim();

            existingEmployee.JobTitle =
                existingEmployee.JobTitle.Trim();

            await _employeeRepository.UpdateAsync(existingEmployee);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee is null)
            {
                return false;
            }

            await _employeeRepository.DeleteAsync(employee);

            return true;
        }

        private static EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? string.Empty
            };
        }
    }
}
