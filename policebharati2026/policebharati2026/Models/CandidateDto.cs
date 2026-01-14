using System.Text.Json.Serialization;

namespace policebharati2026.Models;

public class CandidateDto
{
    [JsonPropertyName("ApplicationNo")]
    public string ApplicationNo { get; set; }

    [JsonPropertyName("Maharashtra_Domicile")]
    public string Maharashtra_Domicile { get; set; }

    [JsonPropertyName("MaharashtraDomicileCertNo")]
    public string MaharashtraDomicileCertNo { get; set; }

    [JsonPropertyName("FarmerSuicideReportNo")]
    public string FarmerSuicideReportNo { get; set; }

    [JsonPropertyName("KarnatakaDomicileCertNo")]
    public string KarnatakaDomicileCertNo { get; set; }

    [JsonPropertyName("NCCCertificateNo")]
    public string NCCCertificateNo { get; set; }

    [JsonPropertyName("CasteCertificateNo")]
    public string CasteCertificateNo { get; set; }

    [JsonPropertyName("Status")]
    public string Status { get; set; }

    [JsonPropertyName("Stage")]
    public string Stage { get; set; }
}