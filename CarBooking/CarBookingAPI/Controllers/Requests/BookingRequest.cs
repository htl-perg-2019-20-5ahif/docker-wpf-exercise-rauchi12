using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingAPI.Controllers
{
    public class BookingRequest
    {
        public int CarId { get; set; }
        public DateTime BookingDate { get; set; }

        public bool IsValid()
        {
            return BookingDate != null && BookingDate.Date > DateTime.Now;
        }
    }
}
