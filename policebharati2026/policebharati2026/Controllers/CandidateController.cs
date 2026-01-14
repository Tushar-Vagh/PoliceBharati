using Microsoft.AspNetCore.Mvc;
using policebharati2026.Models;
using policebharati2026.Services;

namespace policebharati2026.Controllers;

[ApiController]
[Route("api/candidates")]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidateController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCandidate(string id)
    {
        try
        {
            var candidate = await _candidateService.GetCandidateAsync(id);
            if (candidate != null)
            {
                return Ok(candidate);
            }
            return NotFound(new { Message = "Candidate not found" });
        }
        catch (Exception ex)
        {
            return Problem($"Backend error: {ex.Message}");
        }
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyCandidate([FromBody] VerificationRequest req)
    {
        try
        {
            bool success = await _candidateService.VerifyCandidateAsync(req);

            if (success)
                return Ok(new { Message = "Updated successfully", Status = req.Status });
            else
                return NotFound(new { Message = "Candidate not found to update" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem($"Backend error: {ex.Message}");
        }
    }
}