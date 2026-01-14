using Microsoft.AspNetCore.Mvc;
using policebharati2026.Services;
using policebharati2026.DTOs;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/master/bulk-upload")]
    public class MasterBulkUploadController : ControllerBase
    {
        private readonly MasterBulkUploadService _service;

        public MasterBulkUploadController(MasterBulkUploadService service)
        {
            _service = service;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            [FromForm] MasterBulkUploadRequestDto request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Excel file is required");

            var result = await _service.UploadAsync(request.File);
            return Ok(result);
        }
    }
}
