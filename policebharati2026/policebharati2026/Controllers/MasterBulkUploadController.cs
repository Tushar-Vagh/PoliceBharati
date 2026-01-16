using Microsoft.AspNetCore.Mvc;
using policebharati2026.Services;
using policebharati2026.DTOs;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/master/bulk-upload")]

    // ✅ IMPORTANT: Prevent Swagger from breaking on IFormFile
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MasterBulkUploadController : ControllerBase
    {
        private readonly MasterBulkUploadService _service;

        public MasterBulkUploadController(MasterBulkUploadService service)
        {
            _service = service;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return StatusCode(400, new
                {
                    message = "Excel file is required"
                });
            }

            try
            {
                var result = await _service.UploadAsync(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
