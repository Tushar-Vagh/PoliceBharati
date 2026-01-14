using policebharati2026.Models;

namespace policebharati2026.Services;

public interface ICandidateService
{
    Task<CandidateDto?> GetCandidateAsync(string applicationNo);
    Task<bool> VerifyCandidateAsync(VerificationRequest request);
}