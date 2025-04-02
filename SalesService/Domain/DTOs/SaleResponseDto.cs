using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesService.Domain.Entities;

namespace SalesService.Domain.DTOs
{
    public class SaleResponseDto
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public List<SaleNote> Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastStatusChange { get; set; }
    }
}