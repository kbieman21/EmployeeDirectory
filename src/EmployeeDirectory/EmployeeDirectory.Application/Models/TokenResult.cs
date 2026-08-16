using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Models
{
    public sealed class TokenResult
    {
        public string Token { get; init; } = string.Empty;

        public DateTime ExpiresAt { get; init; }
    }
}
