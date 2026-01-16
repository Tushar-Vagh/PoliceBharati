// using System;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;
// using policebharati2026.DTOs;
// using policebharati2026.Services;

// namespace policebharati2026.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class PhysicalStandardController : ControllerBase
//     {
//         private readonly IConfiguration _config;
//         private readonly PhysicalStandardService _service;

//         public PhysicalStandardController(
//             IConfiguration config,
//             PhysicalStandardService service)
//         {
//             _config = config;
//             _service = service;
//         }

//         // ================= EXISTING API (UNCHANGED) =================
//         // POST: api/PhysicalStandard
//         [HttpPost]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//         public async Task<IActionResult> AddStandard(
//             [FromBody] PhysicalStandardRequestDto dto)
//         {
//             bool success = await _service.AddPhysicalStandardAsync(dto);

//             if (success)
//                 return Ok("Record inserted successfully.");

//             return StatusCode(500, "Failed to insert record.");
//         }

//         // ================= STATUS API (UNCHANGED) =================
//         [HttpGet("status/{applicationNo}")]
//         public async Task<IActionResult> GetMeasurementStatus(string applicationNo)
//         {
//             var result = await _service.GetMeasurementStatusAsync(applicationNo);
//             if (result == null) return NotFound();
//             return Ok(result);
//         }

//         // ================= NEW: HEIGHT ONLY =================
//         [HttpPost("height")]
//         public async Task<IActionResult> SubmitHeight([FromBody] HeightDto dto)
//         {
//             await _service.UpdateHeightAsync(dto);
//             return Ok("Height updated successfully.");
//         }

//         // ================= NEW: WEIGHT ONLY =================
//         [HttpPost("weight")]
//         public async Task<IActionResult> SubmitWeight([FromBody] WeightDto dto)
//         {
//             await _service.UpdateWeightAsync(dto);
//             return Ok("Weight updated successfully.");
//         }

//         // ================= NEW: CHEST ONLY =================
//         [HttpPost("chest")]
//         public async Task<IActionResult> SubmitChest([FromBody] ChestDto dto)
//         {
//             await _service.UpdateChestAsync(dto);
//             return Ok("Chest updated successfully.");
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using policebharati2026.DTOs;
using policebharati2026.Services;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhysicalStandardController : ControllerBase
    {
        private readonly PhysicalStandardService _service;

        public PhysicalStandardController(PhysicalStandardService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddStandard([FromBody] PhysicalStandardRequestDto dto)
        {
            bool success = await _service.AddPhysicalStandardAsync(dto);
            return success ? Ok() : StatusCode(500);
        }

        [HttpPost("height")]
        public async Task<IActionResult> SubmitHeight([FromBody] HeightDto dto)
        {
            await _service.UpdateHeightAsync(dto);
            return Ok();
        }

        [HttpPost("weight")]
        public async Task<IActionResult> SubmitWeight([FromBody] WeightDto dto)
        {
            await _service.UpdateWeightAsync(dto);
            return Ok();
        }

        [HttpPost("chest")]
        public async Task<IActionResult> SubmitChest([FromBody] ChestDto dto)
        {
            await _service.UpdateChestAsync(dto);
            return Ok();
        }

        [HttpPost("finalize")]
        public async Task<IActionResult> FinalizePst([FromBody] FinalPstDto dto)
        {
            await _service.FinalizePstAsync(dto);
            return Ok();
        }

        [HttpGet("status/{applicationNo}")]
        public async Task<IActionResult> GetStatus(string applicationNo)
        {
            var result = await _service.GetMeasurementStatusAsync(applicationNo);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{applicationNo}")]
        public async Task<IActionResult> GetPstByApplication(string applicationNo)
        {
            var result = await _service.GetPstByApplicationAsync(applicationNo);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("details/{applicationNo}")]
public async Task<IActionResult> GetPstDetails(string applicationNo)
{
    var result = await _service.GetPstByApplicationAsync(applicationNo);
    if (result == null) return NotFound();
    return Ok(result);
}


    }
}
