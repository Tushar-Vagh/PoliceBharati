using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Data;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void AddSheetFromQuery(
    ExcelPackage package,
    string sheetName,
    string query)
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
        // TOTAL REGISTRATION (NEW)
        // =========================
        [HttpGet("total-registration")]
        public IActionResult DownloadTotalRegistrationExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
                    SELECT 
                        Username,
                        ApplicationNo,
                        CONCAT(
                            FirstName_English, ' ',
                            FatherName_English, ' ',
                            Surname_English
                        ) AS FullName,
                        MotherName_English,
                        Gender,
                        DOB,
                        ApplicationDate
                    FROM master";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound("No records found. Excel file was not generated.");
                }
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Total Registration");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Total_Registration.xlsx"
            );
        }

        // =========================
        // PASSED CANDIDATES (UNCHANGED)
        // =========================
        [HttpGet("passed-candidates")]
        public IActionResult DownloadPassedCandidatesExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
                    SELECT
                        pst.application_number,
                        pst.height_cm,
                        pst.chest_cm,
                        pst.weight_kg,
                        pst.pst_status,
                        COALESCE(
                            LTRIM(RTRIM(m.FirstName_English)) + ' ' +
                            LTRIM(RTRIM(m.FatherName_English)) + ' ' +
                            LTRIM(RTRIM(m.Surname_English)),
                            'NAME NOT FOUND'
                        ) AS Candidate_Name
                    FROM physical_standard_test pst
                    LEFT JOIN Master m
                        ON LTRIM(RTRIM(pst.application_number)) = LTRIM(RTRIM(m.ApplicationNo))
                    WHERE pst.Stage = 1002
                      AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'PASS%'";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound("No records found. Excel file was not generated.");
                }
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Passed Candidates");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "All_Passed_Candidates.xlsx"
            );
        }

        // =========================
        // FAILED CANDIDATES (UNCHANGED)
        // =========================
        [HttpGet("failed-candidates")]
        public IActionResult DownloadFailedCandidatesExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
                    SELECT
                        pst.application_number,
                        pst.height_cm,
                        pst.chest_cm,
                        pst.weight_kg,
                        pst.pst_status,
                        COALESCE(
                            LTRIM(RTRIM(m.FirstName_English)) + ' ' +
                            LTRIM(RTRIM(m.FatherName_English)) + ' ' +
                            LTRIM(RTRIM(m.Surname_English)),
                            'NAME NOT FOUND'
                        ) AS Candidate_Name
                    FROM physical_standard_test pst
                    LEFT JOIN Master m
                        ON LTRIM(RTRIM(pst.application_number)) = LTRIM(RTRIM(m.ApplicationNo))
                    WHERE pst.Stage = 1002
                      AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'FAIL%'";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound("No records found. Excel file was not generated.");
                }
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Failed Candidates");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "All_Failed_Candidates.xlsx"
            );
        }
        // =========================
        // SELECTED CANDIDATES LIST
        // =========================
        [HttpGet("selected-candidates")]
        public IActionResult DownloadSelectedCandidatesExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
                applicationno,
                username,
                CONCAT(
                    FirstName_English, ' ',
                    FatherName_English, ' ',
                    Surname_English
                ) AS FullName,
                MotherName_English,
                Gender,
                DOB,
                ApplicationDate
            FROM master
            WHERE Stage = 1001";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound("No records found. Excel file was not generated.");
                }
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Selected Candidates");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Selected_Candidates_List.xlsx"
            );
        }


        // =========================
        // REJECTED CANDIDATES LIST
        // =========================
        [HttpGet("rejected-candidates")]
        public IActionResult DownloadRejectedCandidatesExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
                applicationno,
                username,
                CONCAT(
                    FirstName_English, ' ',
                    FatherName_English, ' ',
                    Surname_English
                ) AS FullName,
                MotherName_English,
                Gender,
                DOB,
                ApplicationDate
            FROM master
            WHERE Stage = 1004";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound("No records found. Excel file was not generated.");
                }
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Rejected Candidates");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Rejected_Candidates_List.xlsx"
            );
        }

        // =========================
        // VERIFIED CANDIDATES (BIOMETRIC)
        // =========================
        [HttpGet("verified-candidates")]
        public IActionResult DownloadVerifiedCandidatesExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
                UserId,
                Template,
                stage,
                application_no
            FROM Fingerprints
            WHERE stage = 1002";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            // ✅ Validation – no records
            if (dt.Rows.Count == 0)
            {
                return BadRequest("No verified candidates found.");
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Verified Candidates");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Verified_Candidates.xlsx"
            );
        }

        // =========================
        // UNVERIFIED CANDIDATES (BIOMETRIC)
        // =========================
        [HttpGet("unverified-candidates")]
        public IActionResult DownloadUnverifiedCandidatesExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
                UserId,
                Template,
                application_no
            FROM Fingerprints
            WHERE stage = 1004";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            // ✅ Validation – no records
            if (dt.Rows.Count == 0)
            {
                return BadRequest("No unverified candidates found.");
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Unverified Candidates");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Unverified_Candidates.xlsx"
            );
        }

        // =========================
        // PET EVENT WISE REPORT
        // =========================
        [HttpGet("pet-event-wise-report")]
        public IActionResult DownloadPetEventWiseReportExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
                pst_id,
                application_number,
                height_cm,
                chest_cm,
                weight_kg,
                pst_status,
                measured_by_officer_id,
                measurement_date_time,
                remarks,
                receipt_number,
                receipt_generated,
                Stage
            FROM dbo.physical_standard_test";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            // ✅ Validation – no records
            if (dt.Rows.Count == 0)
            {
                return BadRequest("No PET event records found.");
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("PET Event Wise Report");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PET_Event_Wise_Report.xlsx"
            );
        }

        // =========================
        // PET FINAL STATUS – CONSTABLE
        // =========================
        [HttpGet("pet-final-constable")]
        public IActionResult DownloadPetFinalConstableExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
    pet_id, 
    application_no, 
    chest_no, 
    candidate_category, 
    event_name, 
    unit_type, 
    start_time, 
    end_time, 
    time_taken_sec, 
    distance_achieved_m, 
    shot_put_weight_kg, 
    attempt_no, 
    is_best_attempt, 
    marks_awarded, 
    total_event_marks, 
    remarks, 
    recorded_by, 
    recorded_datetime, 
    Stage
FROM dbo.pet_candidate_score 
WHERE chest_no LIKE '%ch%' 
AND (Stage <> 1004 OR Stage IS NULL);";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            // ✅ IMPORTANT VALIDATION
            if (dt.Rows.Count == 0)
                return NotFound("No Constable PET records found");

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Constable PET");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PET_Final_Constable.xlsx"
            );
        }


        // =========================
        // PET FINAL STATUS – DRIVER
        // =========================
        [HttpGet("pet-final-driver")]
        public IActionResult DownloadPetFinalDriverExcel()
        {
            var dt = new DataTable();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT 
                pet_id, application_no, chest_no, candidate_category,
                event_name, unit_type, start_time, end_time,
                time_taken_sec, distance_achieved_m,
                shot_put_weight_kg, attempt_no, is_best_attempt,
                marks_awarded, total_event_marks, remarks,
                recorded_by, recorded_datetime, Stage
            FROM dbo.pet_candidate_score
            WHERE chest_no LIKE '%DR%'
              AND (Stage <> 1004 OR Stage IS NULL);";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            // ✅ Validation if no records
            if (dt.Rows.Count == 0)
                return NotFound("No Driver PET records found");

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Driver PET");

            ws.Cells["A1"].LoadFromDataTable(dt, true);
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PET_Final_Driver.xlsx"
            );
        }




        [HttpGet("audit-report")]
        public IActionResult DownloadAuditReport()
        {
            using var package = new ExcelPackage();

            AddSheetFromQuery(
                package,
                "Total Registration",
               @"
                    SELECT 
                        Username,
                        ApplicationNo,
                        CONCAT(
                            FirstName_English, ' ',
                            FatherName_English, ' ',
                            Surname_English
                        ) AS FullName,
                        MotherName_English,
                        Gender,
                        DOB,
                        ApplicationDate
                    FROM master");

            AddSheetFromQuery(
                package,
                "Selected Candidates",
                 @"
                    SELECT
                        pst.application_number,
                        pst.height_cm,
                        pst.chest_cm,
                        pst.weight_kg,
                        pst.pst_status,
                        COALESCE(
                            LTRIM(RTRIM(m.FirstName_English)) + ' ' +
                            LTRIM(RTRIM(m.FatherName_English)) + ' ' +
                            LTRIM(RTRIM(m.Surname_English)),
                            'NAME NOT FOUND'
                        ) AS Candidate_Name
                    FROM physical_standard_test pst
                    LEFT JOIN Master m
                        ON LTRIM(RTRIM(pst.application_number)) = LTRIM(RTRIM(m.ApplicationNo))
                    WHERE pst.Stage = 1002
                      AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'PASS%'"
            );

            AddSheetFromQuery(
                package,
                "Rejected Candidates",
                @"SELECT applicationno, username,
          CONCAT(FirstName_English,' ',FatherName_English,' ',Surname_English) AS FullName,
          MotherName_English, Gender, DOB, ApplicationDate
          FROM master WHERE Stage = 1004"
            );

            AddSheetFromQuery(
                package,
                "Verified Biometrics",
                @"SELECT UserId, Template, stage, application_no
          FROM Fingerprints WHERE stage = 1002"
            );

            AddSheetFromQuery(
                package,
                "Unverified Biometrics",
                @"SELECT UserId, Template, stage, application_no
          FROM Fingerprints WHERE stage = 1004"
            );

            AddSheetFromQuery(
                package,
                "PET Event Wise",
                @"SELECT * FROM dbo.physical_standard_test"
            );

            AddSheetFromQuery(
                package,
                "All Passed Candidates",
                @"
                    SELECT
                        pst.application_number,
                        pst.height_cm,
                        pst.chest_cm,
                        pst.weight_kg,
                        pst.pst_status,
                        COALESCE(
                            LTRIM(RTRIM(m.FirstName_English)) + ' ' +
                            LTRIM(RTRIM(m.FatherName_English)) + ' ' +
                            LTRIM(RTRIM(m.Surname_English)),
                            'NAME NOT FOUND'
                        ) AS Candidate_Name
                    FROM physical_standard_test pst
                    LEFT JOIN Master m
                        ON LTRIM(RTRIM(pst.application_number)) = LTRIM(RTRIM(m.ApplicationNo))
                    WHERE pst.Stage = 1002
                      AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'PASS%'"

            );

            AddSheetFromQuery(
                package,
                "All Failed Candidates",
                @"
                    SELECT
                        pst.application_number,
                        pst.height_cm,
                        pst.chest_cm,
                        pst.weight_kg,
                        pst.pst_status,
                        COALESCE(
                            LTRIM(RTRIM(m.FirstName_English)) + ' ' +
                            LTRIM(RTRIM(m.FatherName_English)) + ' ' +
                            LTRIM(RTRIM(m.Surname_English)),
                            'NAME NOT FOUND'
                        ) AS Candidate_Name
                    FROM physical_standard_test pst
                    LEFT JOIN Master m
                        ON LTRIM(RTRIM(pst.application_number)) = LTRIM(RTRIM(m.ApplicationNo))
                    WHERE pst.Stage = 1002
                      AND UPPER(LTRIM(RTRIM(pst.pst_status))) LIKE 'FAIL%'"

            );


            AddSheetFromQuery(
                package,
                "PET Final Constable",
                @"
            SELECT 
    pet_id, 
    application_no, 
    chest_no, 
    candidate_category, 
    event_name, 
    unit_type, 
    start_time, 
    end_time, 
    time_taken_sec, 
    distance_achieved_m, 
    shot_put_weight_kg, 
    attempt_no, 
    is_best_attempt, 
    marks_awarded, 
    total_event_marks, 
    remarks, 
    recorded_by, 
    recorded_datetime, 
    Stage
FROM dbo.pet_candidate_score 
WHERE chest_no LIKE '%ch%' 
AND (Stage <> 1004 OR Stage IS NULL);"
            );

            AddSheetFromQuery(
                package,
                "PET Final Driver",
                @"SELECT * FROM dbo.pet_candidate_score
          WHERE chest_no LIKE '%DR%'
          AND (Stage <> 1004 OR Stage IS NULL)"
            );

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Audit_Report.xlsx"
            );
        }


    }
}
