namespace policebharati2026.DTOs
{
    public class PhysicalStandardRequestDto
    {
        public string ApplicationNumber { get; set; }
        public decimal HeightCm { get; set; }
        public decimal? ChestCm { get; set; }
        public decimal WeightKg { get; set; }
        public string Gender { get; set; } // "Male", "Female", "Third"
        public string MeasuredByOfficerId { get; set; }
        public string? Remarks { get; set; }
        public string? ReceiptNumber { get; set; }
    }
}