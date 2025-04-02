using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesService.Domain.DTOs
{
    public class SaleCreateDto
    {
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
    }
}