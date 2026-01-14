using System;

namespace policebharati2026.Models
{
    public class PetCandidateScoreModel
    {
        public long PetId { get; set; }
        public string ApplicationNo { get; set; } = string.Empty;
        public string ChestNo { get; set; } = string.Empty;
        public string CandidateCategory { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string UnitType { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? TimeTakenSec { get; set; }
        public decimal? DistanceAchievedM { get; set; }
        public decimal? ShotPutWeightKg { get; set; }
        public int? AttemptNo { get; set; }
        public string? IsBestAttempt { get; set; }
        public int? MarksAwarded { get; set; }
        public int? TotalEventMarks { get; set; }
        public string? Remarks { get; set; }
        public string RecordedBy { get; set; } = string.Empty;
        public DateTime? RecordedDatetime { get; set; }

        public string? Stage { get; set; }

    }
}
