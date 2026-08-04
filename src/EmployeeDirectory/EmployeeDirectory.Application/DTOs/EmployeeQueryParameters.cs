using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeDirectory.Application.DTOs
{
    public sealed class EmployeeQueryParameters
    {
        private const int MaximumPageSize = 100;
        private int _pageSize = 10;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, MaximumPageSize)]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Min(value, MaximumPageSize);
        }

        public string? Search { get; set; }

        [Range(1, int.MaxValue)]
        public int? DepartmentId { get; set; }

        public string SortBy { get; set; } = "lastName";

        public string SortDirection { get; set; } = "asc";
    }
}
