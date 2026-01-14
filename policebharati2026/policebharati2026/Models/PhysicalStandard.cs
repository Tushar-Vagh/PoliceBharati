namespace policebharati2026.Models
{
    public class PhysicalStandard
    {
        public int PstId { get; set; }
        public string ApplicationNumber { get; set; }
        public decimal HeightCm { get; set; }
        public decimal? ChestCm { get; set; }
        public decimal WeightKg { get; set; }
        public string PstStatus { get; set; }
        public string MeasuredByOfficerId { get; set; }
        public DateTime MeasurementDateTime { get; set; }
        public string? Remarks { get; set; }
        public string? ReceiptNumber { get; set; }
        public bool ReceiptGenerated { get; set; }
        public string Stage { get; set; }
    }
}