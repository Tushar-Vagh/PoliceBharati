using Microsoft.AspNetCore.Mvc;
using policebharati2026.DTOs;
using policebharati2026.Models;
using policebharati2026.Services;
using System.Threading.Tasks;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetCandidateScoreController : ControllerBase
    {
        private readonly PetCandidateScoreService _service;

        public PetCandidateScoreController(PetCandidateScoreService service)
        {
            _service = service;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PetCandidateScoreDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Request body is required." });

            var model = ConvertToModel(dto);
            var ok = await _service.InsertAsync(model);

            if (ok) return Ok(new { message = "Score inserted successfully." });
            return StatusCode(500, new { message = "Failed to insert score." });
        }

        [HttpGet("by-id/{petId:long}")]
        public async Task<IActionResult> GetById(long petId)
        {
            var model = await _service.GetByPetIdAsync(petId);
            if (model == null) return NotFound(new { message = "Record not found." });
            return Ok(model);
        }

        [HttpGet("by-application/{applicationNo}")]
        public async Task<IActionResult> GetByApplication(string applicationNo)
        {
            var model = await _service.GetByApplicationNoAsync(applicationNo);
            if (model == null) return NotFound(new { message = "Record not found." });
            return Ok(model);
        }

        [HttpGet("by-event/{eventName}")]
        public async Task<IActionResult> GetByEvent(string eventName)
        {
            var list = await _service.GetByEventAsync(eventName);
            if (list == null) return NotFound(new { message = "No records found." });
            return Ok(list);
        }

        [HttpPut("update/{petId:long}")]
        public async Task<IActionResult> Update(long petId, [FromBody] PetCandidateScoreDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Request body is required." });

            var exists = await _service.GetByPetIdAsync(petId);
            if (exists == null) return NotFound(new { message = "Record not found." });

            var model = ConvertToModel(dto);
            model.PetId = petId;
            var ok = await _service.UpdateAsync(model);

            if (ok) return Ok(new { message = "Record updated successfully." });
            return StatusCode(500, new { message = "Failed to update record." });
        }

        [HttpDelete("delete/{petId:long}")]
        public async Task<IActionResult> Delete(long petId)
        {
            var exists = await _service.GetByPetIdAsync(petId);
            if (exists == null) return NotFound(new { message = "Record not found." });

            var ok = await _service.DeleteAsync(petId);
            if (ok) return Ok(new { message = "Record deleted successfully." });
            return StatusCode(500, new { message = "Failed to delete record." });
        }
        private PetCandidateScoreModel ConvertToModel(PetCandidateScoreDto dto)
        {
            return new PetCandidateScoreModel
            {
                ApplicationNo = dto.ApplicationNo,
                ChestNo = dto.ChestNo,
                CandidateCategory = dto.CandidateCategory,
                EventName = dto.EventName,
                UnitType = dto.UnitType,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TimeTakenSec = dto.TimeTakenSec,
                DistanceAchievedM = dto.DistanceAchievedM,
                ShotPutWeightKg = dto.ShotPutWeightKg,
                AttemptNo = dto.AttemptNo,
                IsBestAttempt = dto.IsBestAttempt,
                MarksAwarded = dto.MarksAwarded,
                TotalEventMarks = dto.TotalEventMarks,
                Remarks = dto.Remarks,
                RecordedBy = dto.RecordedBy
            };
        }

    }
}
