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
    public class DriverController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DriverController> _logger;
        private readonly IMapper _mapper;

        public DriverController(IUnitOfWork unitOfWork, ILogger<DriverController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDriver()
        {
            try
            {
                var drivers = await _unitOfWork.Driver.GetAll();

                var results = _mapper.Map<IList<DriverDTO>>(drivers);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetDriver)}");
                return StatusCode(500, $"Internal Server Error. Please Try Again Later. {ex}");
            }
        }

        [HttpGet("{id:int}", Name = "GetDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDriver(int id)
        {
            var driver = await _unitOfWork.Driver.Get();
            var result = _mapper.Map<DriverDTO>(driver);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDriver([FromBody] DriverDTO driverDTO)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in the {nameof(CreateDriver)}");
                return BadRequest(ModelState);
            }

            if (await DriverExists(driverDTO.Name)) return BadRequest("Driver is exists");

            else
            {
                try
                {
                    var driver = _mapper.Map<Driver>(driverDTO);

                    await _unitOfWork.Driver.Insert(driver);
                    await _unitOfWork.Save();
                    return CreatedAtRoute("GetDriver", new { id = driver.Id }, driver);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something Went Wrong in the {nameof(CreateDriver)}");
                    return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                }
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverDTO driverDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in the {nameof(UpdateDriver)}");
                return BadRequest(ModelState);
            }

            try
            {
                var driver = await _unitOfWork.Driver.Get(x => x.Id == id);
                if (driver == null)
                {
                    _logger.LogError($"Invalid Update attempt in the {nameof(UpdateDriver)}");
                    return BadRequest("Submited data is invalid");
                }

                _mapper.Map(driverDTO, driver);
                _unitOfWork.Driver.Update(driver);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(UpdateDriver)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        private async Task<bool> DriverExists(string name)
        {
            var employee = await _unitOfWork.Driver.Get(x => x.Name == name);
            if (employee != null) return true;
            return false;
        }
    }
}
