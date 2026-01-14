using Microsoft.Data.SqlClient;
using policebharati2026.DTOs;


namespace policebharati2026.Services


{
    public class PhysicalStandardService
    {
        private readonly IConfiguration _config;

        public PhysicalStandardService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> AddPhysicalStandardAsync(PhysicalStandardRequestDto dto)
        {
            string status = EvaluateStatus(dto.Gender, dto.HeightCm, dto.ChestCm);
            string stage = status == "Pass" ? "1002" : "1004";

            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand(@"
                INSERT INTO physical_standard_test 
                (application_number, height_cm, chest_cm, weight_kg, pst_status, measured_by_officer_id, remarks, receipt_number, Stage)
                VALUES (@appNo, @height, @chest, @weight, @status, @officerId, @remarks, @receipt, @stage)", conn);

            cmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
            cmd.Parameters.AddWithValue("@height", dto.HeightCm);
            cmd.Parameters.AddWithValue("@chest", (object?)dto.ChestCm ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@weight", dto.WeightKg);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@officerId", dto.MeasuredByOfficerId);
            cmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@receipt", (object?)dto.ReceiptNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stage", stage);
    
            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private string EvaluateStatus(string gender, decimal height, decimal? chest)
        {
            gender = gender.ToLower();

            if (gender == "male")
            {
                if (height > 165 && chest.HasValue && chest.Value > 74 && chest.Value < 84)
                    return "Pass";
            }
            else if (gender == "female" || gender == "third")
            {
                if (height > 155)
                    return "Pass";
            }

            return "Fail";
        }
    }
}