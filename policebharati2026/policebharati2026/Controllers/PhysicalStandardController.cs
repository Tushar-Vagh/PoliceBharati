using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using policebharati2026.DTOs;
using policebharati2026.Services;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhysicalStandardController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly PhysicalStandardService _service;

        public PhysicalStandardController(
            IConfiguration config,
            PhysicalStandardService service)
        {
            _config = config;
            _service = service;
        }
    
        // 🔹 POST PST data
        // POST: api/PhysicalStandard
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddStandard(
            [FromBody] PhysicalStandardRequestDto dto)
        {
            bool success = await _service.AddPhysicalStandardAsync(dto);

            if (success)
                return Ok("Record inserted successfully.");

            return StatusCode(500, "Failed to insert record.");
        }
    }
}