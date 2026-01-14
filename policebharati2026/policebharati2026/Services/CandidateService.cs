using Microsoft.Data.SqlClient;
using policebharati2026.Models;

namespace policebharati2026.Services;

public class CandidateService : ICandidateService
{
    private readonly string _connectionString;

    public CandidateService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<CandidateDto?> GetCandidateAsync(string applicationNo)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"
                SELECT 
                    ApplicationNo,
                    Maharashtra_Domicile,
                    MaharashtraDomicileCertNo,
                    FarmerSuicideReportNo,
                    KarnatakaDomicileCertNo,
                    NCCCertificateNo,
                    CasteCertificateNo,
                    status,
                    stage
                FROM master 
                WHERE ApplicationNo = @Id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", applicationNo);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CandidateDto
                {
                    ApplicationNo = reader["ApplicationNo"].ToString(),
                    Maharashtra_Domicile = reader["Maharashtra_Domicile"].ToString(),
                    MaharashtraDomicileCertNo = reader["MaharashtraDomicileCertNo"].ToString(),
                    FarmerSuicideReportNo = reader["FarmerSuicideReportNo"].ToString(),
                    KarnatakaDomicileCertNo = reader["KarnatakaDomicileCertNo"].ToString(),
                    NCCCertificateNo = reader["NCCCertificateNo"].ToString(),
                    CasteCertificateNo = reader["CasteCertificateNo"].ToString(),
                    Status = reader["status"].ToString(),
                    Stage = reader["stage"].ToString()
                };
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DB Error: {ex.Message}");
            throw; 
        }
    }

    public async Task<bool> VerifyCandidateAsync(VerificationRequest request)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        string query = "";
        if (request.Status == "PASS")
        {
            // PASS: Set status Active, clear any failure Stage
            query = "UPDATE master SET status = 'active', stage = NULL WHERE ApplicationNo = @Id";
        }
        else if (request.Status == "FAIL")
        {
            // FAIL: Set Stage 1004, clear Active status
            query = "UPDATE master SET stage = '1004', status = NULL WHERE ApplicationNo = @Id";
        }
        else
        {
            throw new ArgumentException("Invalid Status. Use PASS or FAIL.");
        }

        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Id", request.ApplicationNo);

        int rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }
}