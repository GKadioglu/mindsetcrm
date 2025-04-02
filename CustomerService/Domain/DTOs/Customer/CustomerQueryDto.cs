using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Domain.DTOs.Customer
{
    public class CustomerQueryDto
    {
        public string? Company { get; set; }
        public string? FullName { get; set; }
        public string? SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}