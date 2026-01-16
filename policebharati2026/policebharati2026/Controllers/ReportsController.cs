using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Bouncycastleconnector;   // ✅ REQUIRED
using System.IO;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private IActionResult GenerateSinglePdf(string title, string query, string fileName)
{
    using var stream = new MemoryStream();

    PdfWriter writer = new PdfWriter(stream);
    PdfDocument pdf = new PdfDocument(writer);
    Document document = new Document(pdf,iText.Kernel.Geom.PageSize.A4.Rotate());
    document.SetMargins(20, 20, 20, 20);

    document.Add(new Paragraph(title)
        .SetTextAlignment(TextAlignment.CENTER)
        .SetFontSize(16)
        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)));

    AddPdfTableFromQuery(document, title, query);

    document.Close();

    return File(stream.ToArray(), "application/pdf", fileName);
}



        // =========================
        // EXCEL HELPER (UNCHANGED)
        // =========================
        private void AddSheetFromQuery(ExcelPackage package, string sheetName, string query)
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            var ws = package.Workbook.Worksheets.Add(sheetName);

            if (dt.Rows.Count == 0)
            {
                ws.Cells["A1"].Value = "No records found";
                ws.Cells["A1"].Style.Font.Bold = true;
                return;
            }

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();
        }

        // =========================
        // PDF HELPER (FIXED)
        // =========================
     private void AddPdfTableFromQuery(Document document, string title, string query)
{
    var dt = new DataTable();

    using (SqlConnection con = new SqlConnection(
        _configuration.GetConnectionString("DefaultConnection")))
    {
        using SqlDataAdapter da = new SqlDataAdapter(query, con);
        da.Fill(dt);
    }

    // Fonts
    PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
    PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

    // ===== Section Title =====
    document.Add(
        new Paragraph(title)
            .SetFont(boldFont)
            .SetFontSize(14)
            .SetMarginTop(20)
            .SetMarginBottom(10)
    );

    if (dt.Rows.Count == 0)
    {
        document.Add(new Paragraph("No records found").SetFont(normalFont));
        return;
    }

    // ===== Table =====
    Table table = new Table(dt.Columns.Count)
        .UseAllAvailableWidth()
        .SetKeepTogether(false); // ✅ allows page breaks

    // ===== Header Row =====
    foreach (DataColumn col in dt.Columns)
    {
        Cell headerCell = new Cell()
            .Add(new Paragraph(col.ColumnName)
                .SetFont(boldFont)
                .SetFontSize(9))
            .SetTextAlignment(TextAlignment.CENTER);

        table.AddHeaderCell(headerCell);
    }

    // ===== Data Rows =====
    foreach (DataRow row in dt.Rows)
    {
        foreach (object? value in row.ItemArray)
        {
            table.AddCell(
                new Cell()
                    .Add(new Paragraph(value?.ToString() ?? "")
                        .SetFont(normalFont)
                        .SetFontSize(9))
            );
        }
    }

    document.Add(table);
}


        // =========================
// TOTAL REGISTRATION – PDF
// =========================
[HttpGet("total-registration-pdf")]
public IActionResult TotalRegistrationPdf()
{
    return GenerateSinglePdf(
        "Total Registration",
        "SELECT Username, ApplicationNo, CONCAT(FirstName_English,' ',FatherName_English,' ',Surname_English) AS FullName, MotherName_English, Gender, DOB, ApplicationDate FROM master",
        "Total_Registration.pdf"
    );
}

// =========================
// PASSED CANDIDATES – PDF
// =========================
[HttpGet("passed-candidates-pdf")]
public IActionResult PassedCandidatesPdf()
{
    return GenerateSinglePdf(
        "Passed Candidates",
        "SELECT pst_id,application_number,height_cm,chest_cm,weight_kg,pst_status,chest_no FROM physical_standard_test  WHERE pst_status LIKE 'PASS%'",
        "All_Passed_Candidates.pdf"
    );
}

// =========================
// FAILED CANDIDATES – PDF
// =========================
[HttpGet("failed-candidates-pdf")]
public IActionResult FailedCandidatesPdf()
{
    return GenerateSinglePdf(
        "Failed Candidates",
        "SELECT pst_id,application_number,height_cm,chest_cm,weight_kg,pst_status,chest_no FROM physical_standard_test  WHERE pst_status LIKE 'FAIL%'",
        "All_Failed_Candidates.pdf"
    );
}

// =========================
// SELECTED CANDIDATES – PDF
// =========================
[HttpGet("selected-candidates-pdf")]
public IActionResult SelectedCandidatesPdf()
{
    return GenerateSinglePdf(
        "Selected Candidates",
        "SELECT * FROM master WHERE Stage = 1002",
        "Selected_Candidates_List.pdf"
    );
}

// =========================
// REJECTED CANDIDATES – PDF
// =========================
[HttpGet("rejected-candidates-pdf")]
public IActionResult RejectedCandidatesPdf()
{
    return GenerateSinglePdf(
        "Rejected Candidates",
        "SELECT * FROM master WHERE Stage = 1004",
        "Rejected_Candidates_List.pdf"
    );
}

// =========================
// VERIFIED BIOMETRICS – PDF
// =========================
[HttpGet("verified-candidates-pdf")]
public IActionResult VerifiedCandidatesPdf()
{
    return GenerateSinglePdf(
        "Verified Biometrics",
        "SELECT * FROM Fingerprints WHERE stage = 1002",
        "Verified_Candidates_List.pdf"
    );
}

// =========================
// UNVERIFIED BIOMETRICS – PDF
// =========================
[HttpGet("unverified-candidates-pdf")]
public IActionResult UnverifiedCandidatesPdf()
{
    return GenerateSinglePdf(
        "Unverified Biometrics",
        "SELECT * FROM Fingerprints WHERE stage = 1004",
        "Unverified_Candidates_List.pdf"
    );
}

// =========================
// PET EVENT WISE – PDF
// =========================
[HttpGet("pet-event-wise-report-pdf")]
public IActionResult PetEventWisePdf()
{
    return GenerateSinglePdf(
        "PET Event Wise Report",
        "SELECT pet_id,application_no,chest_no,event_name,start_time,end_time,shot_put_weight_kg,attempt_no,total_event_marks,recorded_datetime FROM pet_candidate_score",
        "PET_Event_Wise_Report.pdf"
    );
}

// =========================
// PET FINAL CONSTABLE – PDF
// =========================
[HttpGet("pet-final-constable-pdf")]
public IActionResult PetFinalConstablePdf()
{ 
    return GenerateSinglePdf(
        "PET Final Constable",
        "SELECT application_no,chest_no, event_name,start_time, end_time,total_event_marks FROM dbo.pet_candidate_score WHERE chest_no LIKE '%ch%' AND (Stage<>1004 OR Stage IS NULL)",
        "PET_Final_Constable.pdf"
    );
}

// =========================
// PET FINAL DRIVER – PDF
// =========================
[HttpGet("pet-final-driver-pdf")]
public IActionResult PetFinalDriverPdf()
{
    return GenerateSinglePdf(
        "PET Final Driver",
        "SELECT application_no,chest_no, event_name,start_time, end_time,total_event_marks FROM pet_candidate_score WHERE chest_no LIKE '%DR%' AND (Stage<>1004 OR Stage IS NULL)",
        "PET_Final_Driver.pdf"
    );
}


        // =========================
        // AUDIT REPORT – EXCEL
        // =========================
        [HttpGet("audit-report")]
        public IActionResult DownloadAuditReportExcel()
        {
            using var package = new ExcelPackage();

            AddSheetFromQuery(package, "Total Registration",
                "SELECT Username, ApplicationNo, CONCAT(FirstName_English,' ',FatherName_English,' ',Surname_English) AS FullName, MotherName_English, Gender, DOB, ApplicationDate FROM master");

            AddSheetFromQuery(package, "Selected Candidates",
                "SELECT pst.application_number, pst.height_cm, pst.chest_cm, pst.weight_kg, pst.pst_status, COALESCE(LTRIM(RTRIM(m.FirstName_English))+' '+LTRIM(RTRIM(m.FatherName_English))+' '+LTRIM(RTRIM(m.Surname_English)),'NAME NOT FOUND') AS Candidate_Name FROM physical_standard_test pst LEFT JOIN Master m ON LTRIM(RTRIM(pst.application_number))=LTRIM(RTRIM(m.ApplicationNo)) WHERE pst.Stage=1002 AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'PASS%'");

            AddSheetFromQuery(package, "Rejected Candidates",
                "SELECT applicationno, username, CONCAT(FirstName_English,' ',FatherName_English,' ',Surname_English) AS FullName, MotherName_English, Gender, DOB, ApplicationDate FROM master WHERE Stage=1004");

            AddSheetFromQuery(package, "Verified Biometrics",
                "SELECT UserId, Template, stage, application_no FROM Fingerprints WHERE stage=1002");

            AddSheetFromQuery(package, "Unverified Biometrics",
                "SELECT UserId, Template, stage, application_no FROM Fingerprints WHERE stage=1004");

            AddSheetFromQuery(package, "PET Final Constable",
                "SELECT * FROM dbo.pet_candidate_score WHERE chest_no LIKE '%ch%' AND (Stage<>1004 OR Stage IS NULL)");

            AddSheetFromQuery(package, "PET Final Driver",
                "SELECT * FROM dbo.pet_candidate_score WHERE chest_no LIKE '%DR%' AND (Stage<>1004 OR Stage IS NULL)");

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Audit_Report.xlsx");
        }

        // =========================
        // AUDIT REPORT – PDF (FIXED)
        // =========================
        [HttpGet("audit-report-pdf")]
        [Produces("application/pdf")]
        public IActionResult DownloadAuditReportPdf()
        {
            using var stream = new MemoryStream();

            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf,iText.Kernel.Geom.PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);

            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            document.Add(new Paragraph("AUDIT REPORT")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetFont(boldFont));

            AddPdfTableFromQuery(document, "Total Registration",
                "SELECT Username, ApplicationNo, CONCAT(FirstName_English,' ',FatherName_English,' ',Surname_English) AS FullName, MotherName_English, Gender, DOB, ApplicationDate FROM master");

            AddPdfTableFromQuery(document, "Passed Candidates",
                "SELECT pst_id,application_number,height_cm,chest_cm,weight_kg,pst_status,chest_no FROM physical_standard_test  WHERE pst_status LIKE 'PASS%'");

            AddPdfTableFromQuery(document, "Failed Candidates",
                "SELECT pst_id,application_number,height_cm,chest_cm,weight_kg,pst_status,chest_no FROM physical_standard_test WHERE pst_status LIKE 'FAIL%'");

            AddPdfTableFromQuery(document, "Selected Candidates",
                "SELECT pst.application_number, pst.height_cm, pst.chest_cm, pst.weight_kg, pst.pst_status, COALESCE(LTRIM(RTRIM(m.FirstName_English))+' '+LTRIM(RTRIM(m.FatherName_English))+' '+LTRIM(RTRIM(m.Surname_English)),'NAME NOT FOUND') AS Candidate_Name FROM physical_standard_test pst LEFT JOIN Master m ON LTRIM(RTRIM(pst.application_number))=LTRIM(RTRIM(m.ApplicationNo)) WHERE pst.Stage=1002 AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'PASS%'");

            AddPdfTableFromQuery(document, "Rejected Candidates",
                "SELECT applicationno, username, CONCAT(FirstName_English,' ',FatherName_English,' ',Surname_English) AS FullName, MotherName_English, Gender, DOB, ApplicationDate FROM master WHERE Stage=1004");

            AddPdfTableFromQuery(document, "Verified Biometrics",
                "SELECT UserId, Template, stage, application_no FROM Fingerprints WHERE stage=1002");

            AddPdfTableFromQuery(document, "Unverified Biometrics",
                "SELECT UserId, Template, stage, application_no FROM Fingerprints WHERE stage=1004");

            AddPdfTableFromQuery(document, "PET Event Wise Report",
            "SELECT pet_id,application_no,chest_no,event_name,start_time,end_time,shot_put_weight_kg,attempt_no,total_event_marks,recorded_datetime FROM pet_candidate_score");

            AddPdfTableFromQuery(document, "PET Final Constable",
                "SELECT application_no,chest_no, event_name,start_time, end_time,total_event_marks FROM dbo.pet_candidate_score WHERE chest_no LIKE '%ch%' AND (Stage<>1004 OR Stage IS NULL)");

            AddPdfTableFromQuery(document, "PET Final Driver",
                "SELECT application_no,chest_no, event_name,start_time, end_time,total_event_marks FROM pet_candidate_score WHERE chest_no LIKE '%DR%' AND (Stage<>1004 OR Stage IS NULL)");

            document.Close();

            return File(stream.ToArray(), "application/pdf", "Audit_Report.pdf");
        }

// =========================
// Final-Selection – PDF
// =========================
[HttpGet("final-selection-pdf")]
public IActionResult FinalSelectionPdf()
{
    return GenerateSinglePdf(
        "Overall Rank / Final Selection",
        "SELECT * FROM pet_candidate_score",
        "Final_Selection.pdf"
    );
}

    }
}
