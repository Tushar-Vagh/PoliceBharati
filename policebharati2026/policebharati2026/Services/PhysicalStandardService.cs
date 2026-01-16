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

        // ================= EXISTING METHOD (UNCHANGED) =================
        public async Task<bool> AddPhysicalStandardAsync(PhysicalStandardRequestDto dto)
        {
            string status = EvaluateStatus(dto.Gender, dto.HeightCm, dto.ChestCm);

            var (measurementStatus, stage) = CalculateStatusAndStage(
                dto.HeightDone,
                dto.WeightDone,
                dto.ChestDone
            );

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand(@"
INSERT INTO physical_standard_test
(application_number, height_cm, chest_cm, weight_kg, pst_status,
 measurement_date_time, remarks, receipt_number, Stage,
 height_done, weight_done, chest_done, measurement_status)
VALUES
(@appNo, @height, @chest, @weight, @status,
 GETDATE(), @remarks, @receipt, @stage,
 @heightDone, @weightDone, @chestDone, @measurementStatus)
", conn);

            cmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
            cmd.Parameters.AddWithValue("@height", dto.HeightCm);
            cmd.Parameters.AddWithValue("@chest", (object?)dto.ChestCm ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@weight", dto.WeightKg);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@receipt", (object?)dto.ReceiptNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@stage", stage);
            cmd.Parameters.AddWithValue("@heightDone", dto.HeightDone);
            cmd.Parameters.AddWithValue("@weightDone", dto.WeightDone);
            cmd.Parameters.AddWithValue("@chestDone", dto.ChestDone);
            cmd.Parameters.AddWithValue("@measurementStatus", measurementStatus);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ================= HEIGHT ONLY =================
        public async Task UpdateHeightAsync(HeightDto dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            var existsCmd = new SqlCommand(
                "SELECT COUNT(1) FROM physical_standard_test WHERE application_number=@appNo",
                conn);
            existsCmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);

            int exists = (int)await existsCmd.ExecuteScalarAsync();

            if (exists == 0)
            {
                var insertCmd = new SqlCommand(@"
INSERT INTO physical_standard_test
(application_number, height_cm, pst_status, height_done, remarks, Stage, measurement_status)
VALUES
(@appNo, @height, 'PENDING', 1, @remarks, '1004', 101)", conn);

                insertCmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
                insertCmd.Parameters.AddWithValue("@height", dto.HeightCm);
                insertCmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);

                await insertCmd.ExecuteNonQueryAsync();
            }
            else
            {
                var updateCmd = new SqlCommand(@"
UPDATE physical_standard_test
SET height_cm = @height,
    height_done = 1,
    remarks = @remarks,
    measurement_status = 101,
    Stage = CASE WHEN weight_done = 1 AND chest_done = 1 THEN '1002' ELSE '1004' END
WHERE application_number = @appNo", conn);

                updateCmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
                updateCmd.Parameters.AddWithValue("@height", dto.HeightCm);
                updateCmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);

                await updateCmd.ExecuteNonQueryAsync();
            }
        }

        // ================= WEIGHT ONLY =================
        public async Task UpdateWeightAsync(WeightDto dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
UPDATE physical_standard_test
SET weight_kg = @weight,
    weight_done = 1,
    remarks = @remarks,
    measurement_status = 102,
    Stage = CASE WHEN height_done = 1 AND chest_done = 1 THEN '1002' ELSE '1004' END,
    measurement_date_time = GETDATE()
WHERE application_number = @appNo", conn);

            cmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
            cmd.Parameters.AddWithValue("@weight", dto.WeightKg);
            cmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        // ================= CHEST ONLY =================
        public async Task UpdateChestAsync(ChestDto dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
UPDATE physical_standard_test
SET chest_cm = @chest,
    chest_done = 1,
    remarks = @remarks,
    measurement_status = 103,
    Stage = CASE WHEN height_done = 1 AND weight_done = 1 THEN '1002' ELSE '1004' END,
    measurement_date_time = GETDATE()
WHERE application_number = @appNo", conn);

            cmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
            cmd.Parameters.AddWithValue("@chest", dto.ChestCm);
            cmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        // ================= FINAL PST =================
//         public async Task FinalizePstAsync(FinalPstDto dto)
//         {
//             using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
//             await conn.OpenAsync();

//             var cmd = new SqlCommand(@"
// UPDATE physical_standard_test
// SET pst_status = @status,
//     receipt_number = @receipt,
//     receipt_generated = 1,
//     remarks = @remarks,
//     Stage = '1002'
// WHERE application_number = @appNo
// ", conn);

//             cmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
//             cmd.Parameters.AddWithValue("@status", dto.PstStatus);
//             cmd.Parameters.AddWithValue("@receipt", dto.ReceiptNumber);
//             cmd.Parameters.AddWithValue(
//     "@remarks",
//     (object?)dto.Remarks ?? DBNull.Value
// );


//             await cmd.ExecuteNonQueryAsync();
//         }


// ================= FINAL PST =================
public async Task FinalizePstAsync(FinalPstDto dto)
{
    using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
    await conn.OpenAsync();

    // 🔹 READ MEASUREMENT COMPLETION
    var readCmd = new SqlCommand(@"
        SELECT height_done, weight_done, chest_done
        FROM physical_standard_test
        WHERE application_number = @appNo
    ", conn);

    readCmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);

    bool heightDone, weightDone, chestDone;

    using (var reader = await readCmd.ExecuteReaderAsync())
    {
        if (!reader.Read())
            throw new Exception("PST record not found");

        heightDone = reader.GetBoolean(0);
        weightDone = reader.GetBoolean(1);
        chestDone  = reader.GetBoolean(2);
    }

    // 🔒 FINAL STAGE RULE (AS REQUESTED)
    string stage =
        heightDone && weightDone && chestDone && dto.PstStatus == "Pass"
            ? "1002"
            : "1004";

    // 🔹 FINAL UPDATE
    var cmd = new SqlCommand(@"
UPDATE physical_standard_test
SET pst_status = @status,
    receipt_number = @receipt,
    receipt_generated = 1,
    remarks = @remarks,
    Stage = @stage
WHERE application_number = @appNo
", conn);

    cmd.Parameters.AddWithValue("@appNo", dto.ApplicationNumber);
    cmd.Parameters.AddWithValue("@status", dto.PstStatus);
    cmd.Parameters.AddWithValue("@receipt", dto.ReceiptNumber);
    cmd.Parameters.AddWithValue("@remarks", (object?)dto.Remarks ?? DBNull.Value);
    cmd.Parameters.AddWithValue("@stage", stage);

    await cmd.ExecuteNonQueryAsync();
}

        // ================= STATUS =================
        public async Task<object?> GetMeasurementStatusAsync(string appNo)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand(@"
SELECT height_done, weight_done, chest_done, Stage
FROM physical_standard_test
WHERE application_number = @appNo", conn);

            cmd.Parameters.AddWithValue("@appNo", appNo);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read()) return null;

            return new
            {
                heightDone = reader.GetBoolean(0),
                weightDone = reader.GetBoolean(1),
                chestDone = reader.GetBoolean(2),
                stage = reader.GetString(3)
            };
        }

        // ================= HELPERS =================
        private string EvaluateStatus(string gender, decimal height, decimal? chest)
        {
            gender = gender?.ToLower();

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

        private (int status, string stage) CalculateStatusAndStage(
            bool heightDone, bool weightDone, bool chestDone)
        {
            if (heightDone && !weightDone && !chestDone) return (101, "1004");
            if (!heightDone && weightDone && !chestDone) return (102, "1004");
            if (!heightDone && !weightDone && chestDone) return (103, "1004");
            if (heightDone && weightDone && chestDone) return (103, "1002");
            return (0, "1004");
        }
public async Task<object?> GetPstByApplicationAsync(string appNo)
{
    using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
    using var cmd = new SqlCommand(@"
        SELECT TOP 1
            height_cm,
            weight_kg,
            chest_cm,
            remarks
        FROM physical_standard_test
        WHERE application_number = @appNo
        ORDER BY pst_id DESC
    ", conn);

    cmd.Parameters.AddWithValue("@appNo", appNo);

    await conn.OpenAsync();
    using var reader = await cmd.ExecuteReaderAsync();

    if (!reader.Read()) return null;

    decimal? height = reader.IsDBNull(0) ? (decimal?)null : reader.GetDecimal(0);
    decimal? weight = reader.IsDBNull(1) ? (decimal?)null : reader.GetDecimal(1);
    decimal? chest  = reader.IsDBNull(2) ? (decimal?)null : reader.GetDecimal(2);
    string? remarks = reader.IsDBNull(3) ? null : reader.GetString(3);

    return new
    {
        heightCm = height,
        weightKg = weight,
        chestCm  = chest,
        remarks  = remarks
    };
}


    }
}
