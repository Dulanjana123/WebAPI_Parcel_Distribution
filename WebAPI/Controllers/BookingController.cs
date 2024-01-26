using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingController> _logger;
        private readonly IMapper _mapper;

        public BookingController(IUnitOfWork unitOfWork, ILogger<BookingController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _unitOfWork.Booking.GetAll(includes: new List<string> { "Driver" });
                
                var results = _mapper.Map<IList<BookingDTO>>(bookings);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooking)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later. {ex}");
            }
        }

        [HttpGet("{id:int}", Name = "GetBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooking(int id)
        {
            var employee = await _unitOfWork.Booking.Get(x => x.Id == id, includes : new List<string> { "Driver" });
            var result = _mapper.Map<BookingDTO>(employee);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO bookingDTO)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in the {nameof(CreateBooking)}");
                return BadRequest(ModelState);
            }

            //if (await DriverExists(bookingDTO.DriverId)) return BadRequest("Driver not available");

            else
            {
                try
                {
                    var booking = _mapper.Map<Booking>(bookingDTO);
                    booking.IsActive = true;
                    await _unitOfWork.Booking.Insert(booking);
                    await _unitOfWork.Save();
                    return CreatedAtRoute("GetBooking", new { id = booking.Id }, booking);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something Went Wrong in the {nameof(CreateBooking)}");
                    return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                }
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] CreateBookingDTO bookingDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in the {nameof(UpdateBooking)}");
                return BadRequest(ModelState);
            }

            try
            {
                var booking = await _unitOfWork.Booking.Get(x => x.Id == id);
                if (booking == null)
                {
                    _logger.LogError($"Invalid Update attempt in the {nameof(UpdateBooking)}");
                    return BadRequest("Submited data is invalid");
                }

                
                _mapper.Map(bookingDTO, booking);
                _unitOfWork.Booking.Update(booking);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(UpdateBooking)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        //private async Task<bool> DriverExists(int driverId)
        //{
        //    var employee = await _unitOfWork.Driver.Get(x => x.Id == driverId);
        //    if (employee != null) return true;
        //    return false;
        //}
    }

    
}
