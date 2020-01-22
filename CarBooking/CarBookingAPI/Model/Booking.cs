using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingAPI.Model
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime BookedDate { get; set; }


        public int CarId { get; set; }

        [Required]
        public Car Car { get; set; }
    }
}
