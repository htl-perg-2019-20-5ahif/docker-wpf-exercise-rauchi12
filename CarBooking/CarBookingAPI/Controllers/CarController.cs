using CarBookingAPI.Data;
using CarBookingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarBookingDataContext _context;

        public CarController(CarBookingDataContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars
                .Include(car => car.Bookings)
                .ToListAsync();
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Car>>> GetAvailableCars([FromQuery] DateTime date)
        {
            return await _context.Cars
                .Include(car => car.Bookings)
                .Where(car =>
                    !car.Bookings.Any(booking => booking.BookedDate > DateTime.Now) &&                                                      // Filter for available
                    (car.Bookings.Count == 0 || car.Bookings.Any(booking => date == null ? booking.BookedDate.Date != date.Date : true))    // Filter for date
                )
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.CarId)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.CarId }, car);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}