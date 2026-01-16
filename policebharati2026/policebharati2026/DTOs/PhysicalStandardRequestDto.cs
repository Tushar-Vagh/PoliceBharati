public class PhysicalStandardRequestDto
{
    public string ApplicationNumber { get; set; }

    public decimal HeightCm { get; set; }
    public decimal? ChestCm { get; set; }
    public decimal? WeightKg { get; set; }

    public string? Gender { get; set; }
    public string? Remarks { get; set; }
    public string? ReceiptNumber { get; set; }

    // measurement completion flags
    public bool HeightDone { get; set; }
    public bool WeightDone { get; set; }
    public bool ChestDone { get; set; }
}
