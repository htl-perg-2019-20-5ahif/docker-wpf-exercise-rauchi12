using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingAPI.Model
{
    public class Car
    {
        public int CarId { get; set; }

        [Required, MaxLength(20)]
        public string Brand { get; set; }

        [Required, MaxLength(20)]
        public string Model { get; set; }

        [Required, MaxLength(20)]
        public string LicensePlaete { get; set; }


        public List<Booking> Bookings { get; set; }
    }
}
