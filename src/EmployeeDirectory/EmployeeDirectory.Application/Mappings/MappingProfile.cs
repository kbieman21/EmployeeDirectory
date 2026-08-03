using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeDirectory.Application.DTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Mappings
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department mappings
            CreateMap<Department, DepartmentDto>();

            CreateMap<CreateDepartmentDto, Department>();

            CreateMap<UpdateDepartmentDto, Department>();

            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    destination => destination.DepartmentName,
                    options => options.MapFrom(
                        source => source.Department != null
                            ? source.Department.Name
                            : string.Empty));

            CreateMap<CreateEmployeeDto, Employee>();

            CreateMap<UpdateEmployeeDto, Employee>();
        }
    }
}
