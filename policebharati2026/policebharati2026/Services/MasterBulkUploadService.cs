using OfficeOpenXml;
using MasterApi.Models;
using policebharati2026.DTOs;
using policebharati2026.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace policebharati2026.Services
{
    public class MasterBulkUploadService
    {
        private readonly SqlHelper _sqlHelper;

        public MasterBulkUploadService(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<MasterBulkUploadResultDto> UploadAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            int total = 0, success = 0, failed = 0;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            using var package = new ExcelPackage(ms);
            var sheet = package.Workbook.Worksheets[0];

            if (sheet.Dimension == null)
                throw new Exception("Empty Excel file");

            // ======================================================
            // ✅ STEP 1: VALIDATE HEADERS (NO EXTRA COLUMNS ALLOWED)
            // ======================================================

            var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Username","TokenNo","ApplicationNo","ApplicationDate","Place","ExamFee","Post","UnitName",
                "FirstName_Marathi","FatherName_Marathi","Surname_Marathi","MotherName_Marathi",
                "FirstName_English","FatherName_English","Surname_English","MotherName_English",
                "Gender","DOB","Religion","Caste","SubCaste",
                "Address1","Address2","Address3","Village1","Mukkam_Post","Taluka","District","State","PinCode",
                "PermanantAddress1","PermanantAddress2","PermanantAddress3","PermanantVillage",
                "PermanantMukkam_Post","PermanantTaluka","PermanantDistrict","PermanantState","PermanantPinCode",
                "EmailID","MobileNo","ApplicationCategory","ParallelReservation",
                "FemaleReservation","NonCreamelayer",
                "Maharashtra_Domicile","MaharashtraDomicileCertNo","MaharashtraDomicileDate",
                "Karnataka_Domicile","KarnatakaDomicileCertNo","KarnatakaDomicileDate",
                "ExSoldier","ExServiceJoiningDate","ExServiceDependent","HomeGuard","Sportsperson","Parttime",
                "ParentInPolice","PoliceRank","PoliceNatureOfEmployment","PoliceDetails",
                "ANATH","AnathDate","AnathCertificateType",
                "IsNCC","NCCCertificateNo","NCCDate",
                "NaxaliteArea","SmallVehicle","Prakalpgrast","Bhukampgrast","WorkonContract",
                "IsFarmerSuicide","FarmerSuicideReportNo","FarmerSuicideReportDate",
                "CasteCertificateNo","CasteCertificateDate",
                "SSCBoardName","SSCResult","SSCMarksObtained","SSCTotalMarks",
                "HSCBoardName","HSCResult","HSCMarksObtained","HSCTotalMarks",
                "SeventhBoardName","SeventhResult","SeventhMarksObtained","SeventhTotalMarks",
                "DiplomaBoardName","DiplomaResult","DiplomaMarksObtained","DiplomaTotalMarks",
                "MSCIT","GraduationDegree","PostGraduationDegree","OtherGraduationDegree","OtherPostGraduationDegree","CreatedDate","UpdatedDate"
            };

            var excelColumns = new List<string>();

            for (int c = 1; c <= sheet.Dimension.End.Column; c++)
            {
                var header = sheet.Cells[1, c].Text?.Trim();
                if (!string.IsNullOrEmpty(header))
                    excelColumns.Add(header);
            }

            var invalidColumns = excelColumns
                .Where(c => !allowedColumns.Contains(c))
                .ToList();

            if (invalidColumns.Any())
            {
                throw new Exception(
                    "Invalid Excel format. Unknown columns: " +
                    string.Join(", ", invalidColumns)
                );
            }

            // ======================================================
            // ✅ STEP 2: PROCESS ROWS (ONLY IF HEADER IS VALID)
            // ======================================================

            for (int r = 2; r <= sheet.Dimension.End.Row; r++)
            {
                total++;
                try
                {
                    var m = new MasterModel
                    {
                        Username = S(sheet, r, 1),
                        TokenNo = S(sheet, r, 2),
                        ApplicationNo = S(sheet, r, 3),
                        ApplicationDate = D(sheet, r, 4),
                        Place = S(sheet, r, 5),
                        ExamFee = Dec(sheet, r, 6),
                        Post = S(sheet, r, 7),
                        UnitName = S(sheet, r, 8),

                        FirstName_Marathi = S(sheet, r, 9),
                        FatherName_Marathi = S(sheet, r, 10),
                        Surname_Marathi = S(sheet, r, 11),
                        MotherName_Marathi = S(sheet, r, 12),

                        FirstName_English = S(sheet, r, 13),
                        FatherName_English = S(sheet, r, 14),
                        Surname_English = S(sheet, r, 15),
                        MotherName_English = S(sheet, r, 16),

                        Gender = S(sheet, r, 17),
                        DOB = D(sheet, r, 18),
                        Religion = S(sheet, r, 19),
                        Caste = S(sheet, r, 20),
                        SubCaste = S(sheet, r, 21),

                        Address1 = S(sheet, r, 22),
                        Address2 = S(sheet, r, 23),
                        Address3 = S(sheet, r, 24),
                        Village1 = S(sheet, r, 25),
                        Mukkam_Post = S(sheet, r, 26),
                        Taluka = S(sheet, r, 27),
                        District = S(sheet, r, 28),
                        State = S(sheet, r, 29),
                        PinCode = S(sheet, r, 30),

                        PermanantAddress1 = S(sheet, r, 31),
                        PermanantAddress2 = S(sheet, r, 32),
                        PermanantAddress3 = S(sheet, r, 33),
                        PermanantVillage = S(sheet, r, 34),
                        PermanantMukkam_Post = S(sheet, r, 35),
                        PermanantTaluka = S(sheet, r, 36),
                        PermanantDistrict = S(sheet, r, 37),
                        PermanantState = S(sheet, r, 38),
                        PermanantPinCode = S(sheet, r, 39),

                        EmailID = S(sheet, r, 40),
                        MobileNo = S(sheet, r, 41),

                        ApplicationCategory = S(sheet, r, 42),
                        ParallelReservation = S(sheet, r, 43),
                        FemaleReservation = B(sheet, r, 44),
                        NonCreamelayer = B(sheet, r, 45),

                        Maharashtra_Domicile = B(sheet, r, 46),
                        MaharashtraDomicileCertNo = S(sheet, r, 47),
                        MaharashtraDomicileDate = D(sheet, r, 48),

                        Karnataka_Domicile = B(sheet, r, 49),
                        KarnatakaDomicileCertNo = S(sheet, r, 50),
                        KarnatakaDomicileDate = D(sheet, r, 51),

                        ExSoldier = B(sheet, r, 52),
                        ExServiceJoiningDate = D(sheet, r, 53),
                        ExServiceDependent = B(sheet, r, 54),
                        HomeGuard = B(sheet, r, 55),
                        Sportsperson = B(sheet, r, 56),
                        Parttime = B(sheet, r, 57),

                        ParentInPolice = B(sheet, r, 58),
                        PoliceRank = S(sheet, r, 59),
                        PoliceNatureOfEmployment = S(sheet, r, 60),
                        PoliceDetails = S(sheet, r, 61),

                        ANATH = B(sheet, r, 62),
                        AnathDate = D(sheet, r, 63),
                        AnathCertificateType = S(sheet, r, 64),

                        IsNCC = B(sheet, r, 65),
                        NCCCertificateNo = S(sheet, r, 66),
                        NCCDate = D(sheet, r, 67),

                        NaxaliteArea = B(sheet, r, 68),
                        SmallVehicle = B(sheet, r, 69),
                        Prakalpgrast = B(sheet, r, 70),
                        Bhukampgrast = B(sheet, r, 71),
                        WorkonContract = B(sheet, r, 72),

                        IsFarmerSuicide = B(sheet, r, 73),
                        FarmerSuicideReportNo = S(sheet, r, 74),
                        FarmerSuicideReportDate = D(sheet, r, 75),

                        CasteCertificateNo = S(sheet, r, 76),
                        CasteCertificateDate = D(sheet, r, 77),

                        SSCBoardName = S(sheet, r, 78),
                        SSCResult = S(sheet, r, 79),
                        SSCMarksObtained = Dec(sheet, r, 80),
                        SSCTotalMarks = Dec(sheet, r, 81),

                        HSCBoardName = S(sheet, r, 82),
                        HSCResult = S(sheet, r, 83),
                        HSCMarksObtained = Dec(sheet, r, 84),
                        HSCTotalMarks = Dec(sheet, r, 85),

                        SeventhBoardName = S(sheet, r, 86),
                        SeventhResult = S(sheet, r, 87),
                        SeventhMarksObtained = Dec(sheet, r, 88),
                        SeventhTotalMarks = Dec(sheet, r, 89),

                        DiplomaBoardName = S(sheet, r, 90),
                        DiplomaResult = S(sheet, r, 91),
                        DiplomaMarksObtained = Dec(sheet, r, 92),
                        DiplomaTotalMarks = Dec(sheet, r, 93),

                        MSCIT = B(sheet, r, 94),
                        GraduationDegree = S(sheet, r, 95),
                        PostGraduationDegree = S(sheet, r, 96),
                        OtherGraduationDegree = S(sheet, r, 97),
                        OtherPostGraduationDegree = S(sheet, r, 98),

                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await _sqlHelper.InsertMasterAsync(m);
                    success++;
                }
                catch
                {
                    failed++;
                }
            }

            return new MasterBulkUploadResultDto
            {
                TotalRows = total,
                Inserted = success,
                Failed = failed
            };
        }

        private string? S(ExcelWorksheet s, int r, int c) => s.Cells[r, c].Text?.Trim();

        private bool B(ExcelWorksheet s, int r, int c)
        {
            var text = s.Cells[r, c].Text?.Trim().ToLower();
            return text switch
            {
                "yes" or "y" or "true" or "1" => true,
                "no" or "n" or "false" or "0" => false,
                _ => false
            };
        }

        private DateTime D(ExcelWorksheet s, int r, int c)
        {
            var cell = s.Cells[r, c];

            if (cell.Value == null)
                return new DateTime(1900, 1, 1);

            if (double.TryParse(cell.Value.ToString(), out double oa))
                return DateTime.FromOADate(oa);

            if (DateTime.TryParse(cell.Value.ToString(), out var dt))
                return dt;

            return new DateTime(1900, 1, 1);
        }

        private decimal Dec(ExcelWorksheet s, int r, int c)
        {
            var cell = s.Cells[r, c];

            if (cell.Value == null)
                return 0;

            if (decimal.TryParse(cell.Value.ToString(), out var v))
                return v;

            if (double.TryParse(cell.Value.ToString(), out var d))
                return Convert.ToDecimal(d);

            return 0;
        }
    }
}
