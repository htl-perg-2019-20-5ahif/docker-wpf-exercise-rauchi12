using CarBookingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingAPI.Data
{
    public class CarBookingDataContext:DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public CarBookingDataContext(DbContextOptions<CarBookingDataContext> options): base(options) {}
    }
}
