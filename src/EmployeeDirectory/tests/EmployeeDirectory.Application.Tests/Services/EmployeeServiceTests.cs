using AutoMapper;
using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Application.Mappings;
using EmployeeDirectory.Application.Services;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EmployeeDirectory.Application.Tests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
    private readonly IMapper _mapper;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _departmentRepositoryMock = new Mock<IDepartmentRepository>();

        var mapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<MappingProfile>();
        }, NullLoggerFactory.Instance);

        _mapper = mapperConfiguration.CreateMapper();

        _service = new EmployeeService(
            _employeeRepositoryMock.Object,
            _departmentRepositoryMock.Object,
            _mapper);

    }


    [Fact]
    public async Task CreateAsync_WhenInputIsValid_CreatesAndReturnsEmployee()
    {
        // Arrange
        var department = new Department
        {
            Id = 2,
            Name = "Information Technology",
            Description = "Technology services"
        };

        var input = new CreateEmployeeDto
        {
            FirstName = " Kibreab ",
            LastName = " Solomon ",
            Email = " kibreab@example.com ",
            JobTitle = " Systems Programmer ",
            HireDate = new DateTime(2021, 8, 1),
            DepartmentId = 2
        };

        _departmentRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(input.DepartmentId))
            .ReturnsAsync(department);

        _employeeRepositoryMock
            .Setup(repository =>
                repository.EmailExistsAsync(input.Email, null))
            .ReturnsAsync(false);

        _employeeRepositoryMock
            .Setup(repository =>
                repository.AddAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee employee) =>
            {
                employee.Id = 10;
                employee.Department = department;
                return employee;
            });

        // Act
        var result = await _service.CreateAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("Kibreab", result.FirstName);
        Assert.Equal("Solomon", result.LastName);
        Assert.Equal("kibreab@example.com", result.Email);
        Assert.Equal("Systems Programmer", result.JobTitle);
        Assert.Equal(2, result.DepartmentId);
        Assert.Equal("Information Technology", result.DepartmentName);

        _employeeRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<Employee>(employee =>
                    employee.FirstName == "Kibreab" &&
                    employee.LastName == "Solomon" &&
                    employee.Email == "kibreab@example.com" &&
                    employee.DepartmentId == 2)),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenDepartmentDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var input = new CreateEmployeeDto
        {
            FirstName = "Kibreab",
            LastName = "Solomon",
            Email = "kibreab@example.com",
            JobTitle = "Systems Programmer",
            HireDate = new DateTime(2021, 8, 1),
            DepartmentId = 999
        };

        _departmentRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(input.DepartmentId))
            .ReturnsAsync((Department?)null);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(input));

        // Assert
        Assert.Equal(
            "Department with ID 999 does not exist.",
            exception.Message);

        _employeeRepositoryMock.Verify(
            repository => repository.AddAsync(It.IsAny<Employee>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var department = new Department
        {
            Id = 2,
            Name = "Information Technology"
        };

        var input = new CreateEmployeeDto
        {
            FirstName = "Kibreab",
            LastName = "Solomon",
            Email = "kibreab@example.com",
            JobTitle = "Systems Programmer",
            HireDate = new DateTime(2021, 8, 1),
            DepartmentId = 2
        };

        _departmentRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(input.DepartmentId))
            .ReturnsAsync(department);

        _employeeRepositoryMock
            .Setup(repository =>
                repository.EmailExistsAsync(input.Email, null))
            .ReturnsAsync(true);

        // Act
        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(input));

        // Assert
        Assert.Equal(
            "An employee with email 'kibreab@example.com' already exists.",
            exception.Message);

        _employeeRepositoryMock.Verify(
            repository => repository.AddAsync(It.IsAny<Employee>()),
            Times.Never);
    }


    [Fact]
    public async Task UpdateAsync_WhenEmployeeDoesNotExist_ReturnsFalse()
    {
        // Arrange
        const int employeeId = 999;

        var input = new UpdateEmployeeDto
        {
            FirstName = "Kibreab",
            LastName = "Solomon",
            Email = "kibreab@example.com",
            JobTitle = "Software Engineer",
            HireDate = new DateTime(2021, 8, 1),
            DepartmentId = 2
        };

        _employeeRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(employeeId))
            .ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.UpdateAsync(employeeId, input);

        // Assert
        Assert.False(result);

        _employeeRepositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<Employee>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenEmployeeExists_DeletesEmployeeAndReturnsTrue()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 5,
            FirstName = "Kibreab",
            LastName = "Solomon",
            Email = "kibreab@example.com",
            JobTitle = "Systems Programmer",
            DepartmentId = 2
        };

        _employeeRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(employee.Id))
            .ReturnsAsync(employee);

        _employeeRepositoryMock
            .Setup(repository =>
                repository.DeleteAsync(employee))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(employee.Id);

        // Assert
        Assert.True(result);

        _employeeRepositoryMock.Verify(
            repository => repository.DeleteAsync(employee),
            Times.Once);
    }




    //[Fact]
    //public void TestDiscovery_Works()
    //{
    //    Assert.True(true);
    //}
}