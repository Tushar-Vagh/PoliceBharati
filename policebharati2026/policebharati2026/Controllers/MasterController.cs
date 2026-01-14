using Microsoft.AspNetCore.Mvc;
using MasterApi.DTOs;
using MasterApi.Services;
using System.Threading.Tasks;

namespace MasterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterController : ControllerBase
    {
        private readonly MasterService _masterService;

        public MasterController(MasterService masterService)
        {
            _masterService = masterService;
        }

        // POST api/master
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MasterDto dto)
        {
            if (dto == null) return BadRequest("Request body is null");
            var created = await _masterService.AddMasterAsync(dto);
            if (created) return Ok(new { message = "Inserted" });
            return StatusCode(500, new { message = "Insert failed" });
        }

        // GET api/master
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _masterService.GetAllAsync();
            return Ok(list);
        }

        // GET api/master/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _masterService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // PUT api/master/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MasterDto dto)
        {
            if (dto == null) return BadRequest("Request body is null");
            var exists = await _masterService.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var updated = await _masterService.UpdateAsync(id, dto);
            if (updated) return Ok(new { message = "Updated" });
            return StatusCode(500, new { message = "Update failed" });
        }
        // GET api/master/by-application/{applicationNo}
        [HttpGet("by-application/{applicationNo}")]
        public async Task<IActionResult> GetByApplicationNo(string applicationNo)
        {
            if (string.IsNullOrWhiteSpace(applicationNo))
                return BadRequest(new { message = "Application number is required" });

            var item = await _masterService.GetByApplicationNoAsync(applicationNo);

            if (item == null)
                return NotFound(new { message = "Application not found" });

            return Ok(item);
        }

        // DELETE api/master/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _masterService.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var deleted = await _masterService.DeleteAsync(id);
            if (deleted) return Ok(new { message = "Deleted" });
            return StatusCode(500, new { message = "Delete failed" });
        }
    }
}
