//using Microsoft.Data.SqlClient;
//using policebharati2026.DTOs;
//using policebharati2026.Models;
//using policebharati2026.Data;
//using System.Threading.Tasks;

//namespace policebharati2026.Data

//{
//    public class PetCandidateScoreService
//    {
//        public async Task<bool> InsertAsync(PetCandidateScoreDto dto)
//        {
//            const string sql = @"
//INSERT INTO pet_candidate_score (
//    application_no, chest_no, candidate_category, event_name, unit_type,
//    start_time, end_time, time_taken_sec, distance_achieved_m, shot_put_weight_kg,
//    attempt_no, is_best_attempt, marks_awarded, total_event_marks, remarks, recorded_by
//) VALUES (
//    @application_no, @chest_no, @candidate_category, @event_name, @unit_type,
//    @start_time, @end_time, @time_taken_sec, @distance_achieved_m, @shot_put_weight_kg,
//    @attempt_no, @is_best_attempt, @marks_awarded, @total_event_marks, @remarks, @recorded_by
//)";

//            var parameters = new[]
//            {
//                new SqlParameter("@application_no", dto.ApplicationNo),
//                new SqlParameter("@chest_no", dto.ChestNo),
//                new SqlParameter("@candidate_category", dto.CandidateCategory),
//                new SqlParameter("@event_name", dto.EventName),
//                new SqlParameter("@unit_type", dto.UnitType),
//                new SqlParameter("@start_time", dto.StartTime ?? (object)DBNull.Value),
//                new SqlParameter("@end_time", dto.EndTime ?? (object)DBNull.Value),
//                new SqlParameter("@time_taken_sec", dto.TimeTakenSec ?? (object)DBNull.Value),
//                new SqlParameter("@distance_achieved_m", dto.DistanceAchievedM ?? (object)DBNull.Value),
//                new SqlParameter("@shot_put_weight_kg", dto.ShotPutWeightKg ?? (object)DBNull.Value),
//                new SqlParameter("@attempt_no", dto.AttemptNo ?? (object)DBNull.Value),
//                new SqlParameter("@is_best_attempt", (object?)dto.IsBestAttempt ?? DBNull.Value),
//                new SqlParameter("@marks_awarded", dto.MarksAwarded ?? (object)DBNull.Value),
//                new SqlParameter("@total_event_marks", dto.TotalEventMarks ?? (object)DBNull.Value),
//                new SqlParameter("@remarks", (object?)dto.Remarks ?? DBNull.Value),
//                new SqlParameter("@recorded_by", dto.RecordedBy)
//            };

//            var rows = await SqlHelper.ExecuteNonQueryAsync(sql, parameters);
//            return rows > 0;
//        }

//        public async Task<PetCandidateScoreModel?> GetByPetIdAsync(long petId)
//        {
//            const string sql = "SELECT * FROM pet_candidate_score WHERE pet_id = @pet_id";
//            await using var conn = SqlHelper.GetConnection();
//            await using var cmd = new SqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("@pet_id", petId);
//            await conn.OpenAsync();
//            await using var reader = await cmd.ExecuteReaderAsync();
//            if (!await reader.ReadAsync()) return null;
//            return MapReader(reader);
//        }

//        public async Task<PetCandidateScoreModel?> GetByApplicationNoAsync(string applicationNo)
//        {
//            const string sql = "SELECT * FROM pet_candidate_score WHERE application_no = @application_no";
//            await using var conn = SqlHelper.GetConnection();
//            await using var cmd = new SqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("@application_no", applicationNo);
//            await conn.OpenAsync();
//            await using var reader = await cmd.ExecuteReaderAsync();
//            if (!await reader.ReadAsync()) return null;
//            return MapReader(reader);
//        }

//        public async Task<IEnumerable<PetCandidateScoreModel>> GetByEventAsync(string eventName)
//        {
//            const string sql = "SELECT * FROM pet_candidate_score WHERE event_name = @event_name ORDER BY pet_id";
//            var list = new List<PetCandidateScoreModel>();
//            await using var conn = SqlHelper.GetConnection();
//            await using var cmd = new SqlCommand(sql, conn);
//            cmd.Parameters.AddWithValue("@event_name", eventName);
//            await conn.OpenAsync();
//            await using var reader = await cmd.ExecuteReaderAsync();
//            while (await reader.ReadAsync())
//            {
//                list.Add(MapReader(reader));
//            }
//            return list;
//        }

//        public async Task<bool> UpdateAsync(long petId, PetCandidateScoreDto dto)
//        {
//            const string sql = @"
//UPDATE pet_candidate_score SET
//    application_no = @application_no,
//    chest_no = @chest_no,
//    candidate_category = @candidate_category,
//    event_name = @event_name,
//    unit_type = @unit_type,
//    start_time = @start_time,
//    end_time = @end_time,
//    time_taken_sec = @time_taken_sec,
//    distance_achieved_m = @distance_achieved_m,
//    shot_put_weight_kg = @shot_put_weight_kg,
//    attempt_no = @attempt_no,
//    is_best_attempt = @is_best_attempt,
//    marks_awarded = @marks_awarded,
//    total_event_marks = @total_event_marks,
//    remarks = @remarks,
//    recorded_by = @recorded_by
//WHERE pet_id = @pet_id";

//            var parameters = new[]
//            {
//                new SqlParameter("@application_no", dto.ApplicationNo),
//                new SqlParameter("@chest_no", dto.ChestNo),
//                new SqlParameter("@candidate_category", dto.CandidateCategory),
//                new SqlParameter("@event_name", dto.EventName),
//                new SqlParameter("@unit_type", dto.UnitType),
//                new SqlParameter("@start_time", dto.StartTime ?? (object)DBNull.Value),
//                new SqlParameter("@end_time", dto.EndTime ?? (object)DBNull.Value),
//                new SqlParameter("@time_taken_sec", dto.TimeTakenSec ?? (object)DBNull.Value),
//                new SqlParameter("@distance_achieved_m", dto.DistanceAchievedM ?? (object)DBNull.Value),
//                new SqlParameter("@shot_put_weight_kg", dto.ShotPutWeightKg ?? (object)DBNull.Value),
//                new SqlParameter("@attempt_no", dto.AttemptNo ?? (object)DBNull.Value),
//                new SqlParameter("@is_best_attempt", (object?)dto.IsBestAttempt ?? DBNull.Value),
//                new SqlParameter("@marks_awarded", dto.MarksAwarded ?? (object)DBNull.Value),
//                new SqlParameter("@total_event_marks", dto.TotalEventMarks ?? (object)DBNull.Value),
//                new SqlParameter("@remarks", (object?)dto.Remarks ?? DBNull.Value),
//                new SqlParameter("@recorded_by", dto.RecordedBy),
//                new SqlParameter("@pet_id", petId)
//            };

//            var rows = await SqlHelper.ExecuteNonQueryAsync(sql, parameters);
//            return rows > 0;
//        }

//        public async Task<bool> DeleteAsync(long petId)
//        {
//            const string sql = "DELETE FROM pet_candidate_score WHERE pet_id = @pet_id";
//            var rows = await SqlHelper.ExecuteNonQueryAsync(sql, new SqlParameter("@pet_id", petId));
//            return rows > 0;
//        }

//        private PetCandidateScoreModel MapReader(SqlDataReader reader)
//        {
//            return new PetCandidateScoreModel
//            {
//                PetId = reader.GetInt64(reader.GetOrdinal("pet_id")),
//                ApplicationNo = reader["application_no"] as string ?? string.Empty,
//                ChestNo = reader["chest_no"] as string ?? string.Empty,
//                CandidateCategory = reader["candidate_category"] as string ?? string.Empty,
//                EventName = reader["event_name"] as string ?? string.Empty,
//                UnitType = reader["unit_type"] as string ?? string.Empty,
//                StartTime = reader["start_time"] as DateTime?,
//                EndTime = reader["end_time"] as DateTime?,
//                TimeTakenSec = reader["time_taken_sec"] as decimal?,
//                DistanceAchievedM = reader["distance_achieved_m"] as decimal?,
//                ShotPutWeightKg = reader["shot_put_weight_kg"] as decimal?,
//                AttemptNo = reader["attempt_no"] as int?,
//                IsBestAttempt = reader["is_best_attempt"] as string?,
//                MarksAwarded = reader["marks_awarded"] as int?,
//                TotalEventMarks = reader["total_event_marks"] as int?,
//                Remarks = reader["remarks"] as string?,
//                RecordedBy = reader["recorded_by"] as string ?? string.Empty,
//                RecordedDatetime = reader["recorded_datetime"] as DateTime?
//            };
//        }
//    }
//}
 // ✅ use the same namespace as your SqlHelper
using policebharati2026.Data;
using policebharati2026.DTOs;
using policebharati2026.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace policebharati2026.Services
{
    public class PetCandidateScoreService
    {
        private readonly SqlHelper _sqlHelper;

        // ✅ inject SqlHelper via constructor (matches how you registered it in Program.cs)
        public PetCandidateScoreService(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public Task<bool> InsertAsync(PetCandidateScoreModel model) =>
            _sqlHelper.InsertPetCandidateScoreAsync(model);

        public Task<PetCandidateScoreModel?> GetByPetIdAsync(long petId) =>
            _sqlHelper.GetPetCandidateScoreByIdAsync(petId);

        public Task<PetCandidateScoreModel?> GetByApplicationNoAsync(string applicationNo) =>
            _sqlHelper.GetPetCandidateScoreByApplicationNoAsync(applicationNo);

        public Task<List<PetCandidateScoreModel>> GetByEventAsync(string eventName) =>
            _sqlHelper.GetPetCandidateScoresByEventAsync(eventName);

        public Task<bool> UpdateAsync(PetCandidateScoreModel model) =>
            _sqlHelper.UpdatePetCandidateScoreAsync(model);

        public Task<bool> DeleteAsync(long petId) =>
            _sqlHelper.DeletePetCandidateScoreAsync(petId);
    }
}
