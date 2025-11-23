using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteStore.Application.DTOs.Payment
{
    public class PaymentIntentRequestDto
    {
        public string CartId { get; set; } = string.Empty;
    }
}

