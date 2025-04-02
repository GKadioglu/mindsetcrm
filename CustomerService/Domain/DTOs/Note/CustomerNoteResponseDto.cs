using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Domain.DTOs.Note
{
    public class CustomerNoteResponseDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}