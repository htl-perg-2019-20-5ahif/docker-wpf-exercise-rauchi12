using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarBookingAPI.Data;
using CarBookingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly CarBookingDataContext _context;

        public BookingController(CarBookingDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.Include(booking => booking.Car).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Car>> PostBooking([FromBody] BookingRequest bookingRequest)
        {

            if (!bookingRequest.IsValid())
            {
                return BadRequest("Invalid request object.");
            }

            if (!_context.Cars.Any(car => car.CarId == bookingRequest.CarId))
            {
                return BadRequest("Invalid car id.");
            }

            _context.Bookings.Add(new Booking
            {
                CarId = bookingRequest.CarId,
                BookedDate = bookingRequest.BookingDate.Date
            });


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Booking>> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }


        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}