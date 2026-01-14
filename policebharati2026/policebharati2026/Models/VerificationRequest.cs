using System.Text.Json.Serialization;

namespace policebharati2026.Models;

public class VerificationRequest
{
    [JsonPropertyName("ApplicationNo")]
    public required string ApplicationNo { get; set; }

    [JsonPropertyName("Status")]
    public required string Status { get; set; }
}