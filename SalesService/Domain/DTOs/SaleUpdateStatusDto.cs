using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesService.Domain.DTOs
{
    public class SaleUpdateStatusDto
    {
        public string NewStatus { get; set; } = string.Empty;
        public string? Note { get; set; } // Not opsiyonel olabilir
    }
}