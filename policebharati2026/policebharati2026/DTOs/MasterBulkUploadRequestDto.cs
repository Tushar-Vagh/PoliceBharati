using Microsoft.AspNetCore.Http;

namespace policebharati2026.DTOs
{
    public class MasterBulkUploadRequestDto
    {
        public IFormFile File { get; set; } = default!;
    }
}
