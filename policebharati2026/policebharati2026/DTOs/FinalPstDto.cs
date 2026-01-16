public class FinalPstDto
{
    public string ApplicationNumber { get; set; }
    public string PstStatus { get; set; }   // Pass / Fail
    public string ReceiptNumber { get; set; }
    public string? Remarks { get; set; }    // ✅ ADD THIS
}
