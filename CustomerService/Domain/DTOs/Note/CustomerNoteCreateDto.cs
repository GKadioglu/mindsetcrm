using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Domain.DTOs.Note
{
    public class CustomerNoteCreateDto
    {
        public Guid CustomerId { get; set; }
        public string Note { get; set; }
    }
}