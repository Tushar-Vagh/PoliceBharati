using MasterApi.Models;
using Microsoft.Extensions.Configuration;
using policebharati2026.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace policebharati2026.Data
{
    public class SqlHelper
    {
        private readonly IConfiguration _configuration;

        public SqlHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string ConnectionString => _configuration.GetConnectionString("DefaultConnection");

        private static void AddParam(SqlCommand cmd, string name, object? value, SqlDbType? dbType = null)
        {
            var param = cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            if (dbType.HasValue)
                param.SqlDbType = dbType.Value;
        }

        public async Task<bool> InsertMasterAsync(MasterModel model)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = @"
INSERT INTO Master (
 Username, TokenNo, ApplicationNo, ApplicationDate, Place, ExamFee, Post, UnitName,
 FirstName_Marathi, FatherName_Marathi, Surname_Marathi, MotherName_Marathi,
 FirstName_English, FatherName_English, Surname_English, MotherName_English,
 Gender, DOB, Religion, Caste, SubCaste, Address1, Address2, Address3, Village1,
 Mukkam_Post, Taluka, District, State, PinCode,
 PermanantAddress1, PermanantAddress2, PermanantAddress3, PermanantVillage, PermanantMukkam_Post, PermanantTaluka, PermanantDistrict, PermanantState, PermanantPinCode,
 EmailID, MobileNo, ApplicationCategory, ParallelReservation, FemaleReservation, NonCreamelayer,
 Maharashtra_Domicile, MaharashtraDomicileCertNo, MaharashtraDomicileDate,
 Karnataka_Domicile, KarnatakaDomicileCertNo, KarnatakaDomicileDate,
 ExSoldier, ExServiceJoiningDate, ExServiceDependent, HomeGuard, Sportsperson, Parttime,
 ParentInPolice, PoliceRank, PoliceNatureOfEmployment, PoliceDetails,
 ANATH, AnathDate, AnathCertificateType,
 IsNCC, NCCCertificateNo, NCCDate,
 NaxaliteArea, SmallVehicle, Prakalpgrast, Bhukampgrast, WorkonContract,
 IsFarmerSuicide, FarmerSuicideReportNo, FarmerSuicideReportDate,
 CasteCertificateNo, CasteCertificateDate,
 SSCBoardName, SSCResult, SSCMarksObtained, SSCTotalMarks,
 HSCBoardName, HSCResult, HSCMarksObtained, HSCTotalMarks,
 SeventhBoardName, SeventhResult, SeventhMarksObtained, SeventhTotalMarks,
 DiplomaBoardName, DiplomaResult, DiplomaMarksObtained, DiplomaTotalMarks,
 MSCIT, GraduationDegree, PostGraduationDegree, OtherGraduationDegree, OtherPostGraduationDegree,
 CreatedDate, UpdatedDate
) VALUES (
 @Username, @TokenNo, @ApplicationNo, @ApplicationDate, @Place, @ExamFee, @Post, @UnitName,
 @FirstName_Marathi, @FatherName_Marathi, @Surname_Marathi, @MotherName_Marathi,
 @FirstName_English, @FatherName_English, @Surname_English, @MotherName_English,
 @Gender, @DOB, @Religion, @Caste, @SubCaste, @Address1, @Address2, @Address3, @Village1,
 @Mukkam_Post, @Taluka, @District, @State, @PinCode,
 @PermanantAddress1, @PermanantAddress2, @PermanantAddress3, @PermanantVillage, @PermanantMukkam_Post, @PermanantTaluka, @PermanantDistrict, @PermanantState, @PermanantPinCode,
 @EmailID, @MobileNo, @ApplicationCategory, @ParallelReservation, @FemaleReservation, @NonCreamelayer,
 @Maharashtra_Domicile, @MaharashtraDomicileCertNo, @MaharashtraDomicileDate,
 @Karnataka_Domicile, @KarnatakaDomicileCertNo, @KarnatakaDomicileDate,
 @ExSoldier, @ExServiceJoiningDate, @ExServiceDependent, @HomeGuard, @Sportsperson, @Parttime,
 @ParentInPolice, @PoliceRank, @PoliceNatureOfEmployment, @PoliceDetails,
 @ANATH, @AnathDate, @AnathCertificateType,
 @IsNCC, @NCCCertificateNo, @NCCDate,
 @NaxaliteArea, @SmallVehicle, @Prakalpgrast, @Bhukampgrast, @WorkonContract,
 @IsFarmerSuicide, @FarmerSuicideReportNo, @FarmerSuicideReportDate,
 @CasteCertificateNo, @CasteCertificateDate,
 @SSCBoardName, @SSCResult, @SSCMarksObtained, @SSCTotalMarks,
 @HSCBoardName, @HSCResult, @HSCMarksObtained, @HSCTotalMarks,
 @SeventhBoardName, @SeventhResult, @SeventhMarksObtained, @SeventhTotalMarks,
 @DiplomaBoardName, @DiplomaResult, @DiplomaMarksObtained, @DiplomaTotalMarks,
 @MSCIT, @GraduationDegree, @PostGraduationDegree, @OtherGraduationDegree, @OtherPostGraduationDegree,
 @CreatedDate, @UpdatedDate
);";

            using var cmd = new SqlCommand(sql, conn);

            AddParam(cmd, "@Username", model.Username);
            AddParam(cmd, "@TokenNo", model.TokenNo);
            AddParam(cmd, "@ApplicationNo", model.ApplicationNo);
            AddParam(cmd, "@ApplicationDate", model.ApplicationDate);
            AddParam(cmd, "@Place", model.Place);
            AddParam(cmd, "@ExamFee", model.ExamFee);
            AddParam(cmd, "@Post", model.Post);
            AddParam(cmd, "@UnitName", model.UnitName);

            AddParam(cmd, "@FirstName_Marathi", model.FirstName_Marathi);
            AddParam(cmd, "@FatherName_Marathi", model.FatherName_Marathi);
            AddParam(cmd, "@Surname_Marathi", model.Surname_Marathi);
            AddParam(cmd, "@MotherName_Marathi", model.MotherName_Marathi);

            AddParam(cmd, "@FirstName_English", model.FirstName_English);
            AddParam(cmd, "@FatherName_English", model.FatherName_English);
            AddParam(cmd, "@Surname_English", model.Surname_English);
            AddParam(cmd, "@MotherName_English", model.MotherName_English);

            AddParam(cmd, "@Gender", model.Gender);
            AddParam(cmd, "@DOB", model.DOB);
            AddParam(cmd, "@Religion", model.Religion);
            AddParam(cmd, "@Caste", model.Caste);
            AddParam(cmd, "@SubCaste", model.SubCaste);

            AddParam(cmd, "@Address1", model.Address1);
            AddParam(cmd, "@Address2", model.Address2);
            AddParam(cmd, "@Address3", model.Address3);
            AddParam(cmd, "@Village1", model.Village1);
            AddParam(cmd, "@Mukkam_Post", model.Mukkam_Post);
            AddParam(cmd, "@Taluka", model.Taluka);
            AddParam(cmd, "@District", model.District);
            AddParam(cmd, "@State", model.State);
            AddParam(cmd, "@PinCode", model.PinCode);

            AddParam(cmd, "@PermanantAddress1", model.PermanantAddress1);
            AddParam(cmd, "@PermanantAddress2", model.PermanantAddress2);
            AddParam(cmd, "@PermanantAddress3", model.PermanantAddress3);
            AddParam(cmd, "@PermanantVillage", model.PermanantVillage);
            AddParam(cmd, "@PermanantMukkam_Post", model.PermanantMukkam_Post);
            AddParam(cmd, "@PermanantTaluka", model.PermanantTaluka);
            AddParam(cmd, "@PermanantDistrict", model.PermanantDistrict);
            AddParam(cmd, "@PermanantState", model.PermanantState);
            AddParam(cmd, "@PermanantPinCode", model.PermanantPinCode);

            AddParam(cmd, "@EmailID", model.EmailID);
            AddParam(cmd, "@MobileNo", model.MobileNo);
            AddParam(cmd, "@ApplicationCategory", model.ApplicationCategory);
            AddParam(cmd, "@ParallelReservation", model.ParallelReservation);
            AddParam(cmd, "@FemaleReservation", model.FemaleReservation, SqlDbType.Bit);
            AddParam(cmd, "@NonCreamelayer", model.NonCreamelayer, SqlDbType.Bit);

            AddParam(cmd, "@Maharashtra_Domicile", model.Maharashtra_Domicile, SqlDbType.Bit);
            AddParam(cmd, "@MaharashtraDomicileCertNo", model.MaharashtraDomicileCertNo);
            AddParam(cmd, "@MaharashtraDomicileDate", model.MaharashtraDomicileDate);
            AddParam(cmd, "@Karnataka_Domicile", model.Karnataka_Domicile, SqlDbType.Bit);
            AddParam(cmd, "@KarnatakaDomicileCertNo", model.KarnatakaDomicileCertNo);
            AddParam(cmd, "@KarnatakaDomicileDate", model.KarnatakaDomicileDate);

            AddParam(cmd, "@ExSoldier", model.ExSoldier, SqlDbType.Bit);
            AddParam(cmd, "@ExServiceJoiningDate", model.ExServiceJoiningDate);
            AddParam(cmd, "@ExServiceDependent", model.ExServiceDependent, SqlDbType.Bit);
            AddParam(cmd, "@HomeGuard", model.HomeGuard, SqlDbType.Bit);
            AddParam(cmd, "@Sportsperson", model.Sportsperson, SqlDbType.Bit);
            AddParam(cmd, "@Parttime", model.Parttime, SqlDbType.Bit);

            AddParam(cmd, "@ParentInPolice", model.ParentInPolice, SqlDbType.Bit);
            AddParam(cmd, "@PoliceRank", model.PoliceRank);
            AddParam(cmd, "@PoliceNatureOfEmployment", model.PoliceNatureOfEmployment);
            AddParam(cmd, "@PoliceDetails", model.PoliceDetails);

            AddParam(cmd, "@ANATH", model.ANATH, SqlDbType.Bit);
            AddParam(cmd, "@AnathDate", model.AnathDate);
            AddParam(cmd, "@AnathCertificateType", model.AnathCertificateType);

            AddParam(cmd, "@IsNCC", model.IsNCC, SqlDbType.Bit);
            AddParam(cmd, "@NCCCertificateNo", model.NCCCertificateNo);
            AddParam(cmd, "@NCCDate", model.NCCDate);

            AddParam(cmd, "@NaxaliteArea", model.NaxaliteArea, SqlDbType.Bit);
            AddParam(cmd, "@SmallVehicle", model.SmallVehicle, SqlDbType.Bit);
            AddParam(cmd, "@Prakalpgrast", model.Prakalpgrast, SqlDbType.Bit);
            AddParam(cmd, "@Bhukampgrast", model.Bhukampgrast, SqlDbType.Bit);
            AddParam(cmd, "@WorkonContract", model.WorkonContract, SqlDbType.Bit);

            AddParam(cmd, "@IsFarmerSuicide", model.IsFarmerSuicide, SqlDbType.Bit);
            AddParam(cmd, "@FarmerSuicideReportNo", model.FarmerSuicideReportNo);
            AddParam(cmd, "@FarmerSuicideReportDate", model.FarmerSuicideReportDate);

            AddParam(cmd, "@CasteCertificateNo", model.CasteCertificateNo);
            AddParam(cmd, "@CasteCertificateDate", model.CasteCertificateDate);

            AddParam(cmd, "@SSCBoardName", model.SSCBoardName);
            AddParam(cmd, "@SSCResult", model.SSCResult);
            AddParam(cmd, "@SSCMarksObtained", model.SSCMarksObtained);
            AddParam(cmd, "@SSCTotalMarks", model.SSCTotalMarks);

            AddParam(cmd, "@HSCBoardName", model.HSCBoardName);
            AddParam(cmd, "@HSCResult", model.HSCResult);
            AddParam(cmd, "@HSCMarksObtained", model.HSCMarksObtained);
            AddParam(cmd, "@HSCTotalMarks", model.HSCTotalMarks);

            AddParam(cmd, "@SeventhBoardName", model.SeventhBoardName);
            AddParam(cmd, "@SeventhResult", model.SeventhResult);
            AddParam(cmd, "@SeventhMarksObtained", model.SeventhMarksObtained);
            AddParam(cmd, "@SeventhTotalMarks", model.SeventhTotalMarks);

            AddParam(cmd, "@DiplomaBoardName", model.DiplomaBoardName);
            AddParam(cmd, "@DiplomaResult", model.DiplomaResult);
            AddParam(cmd, "@DiplomaMarksObtained", model.DiplomaMarksObtained);
            AddParam(cmd, "@DiplomaTotalMarks", model.DiplomaTotalMarks);

            AddParam(cmd, "@MSCIT", model.MSCIT, SqlDbType.Bit);
            AddParam(cmd, "@GraduationDegree", model.GraduationDegree);
            AddParam(cmd, "@PostGraduationDegree", model.PostGraduationDegree);
            AddParam(cmd, "@OtherGraduationDegree", model.OtherGraduationDegree);
            AddParam(cmd, "@OtherPostGraduationDegree", model.OtherPostGraduationDegree);

            AddParam(cmd, "@CreatedDate", model.CreatedDate ?? DateTime.UtcNow);
            AddParam(cmd, "@UpdatedDate", model.UpdatedDate ?? DateTime.UtcNow);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<List<MasterModel>> GetAllMastersAsync()
        {
            var list = new List<MasterModel>();
            using var conn = new SqlConnection(ConnectionString);
            var sql = "SELECT * FROM Master";
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReaderToModel(reader));
            }
            return list;
        }

        public async Task<MasterModel?> GetMasterByIdAsync(int srNo)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = "SELECT * FROM Master WHERE SrNo = @SrNo";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@SrNo", srNo, SqlDbType.Int);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapReaderToModel(reader);
            return null;
        }

        public async Task<bool> UpdateMasterAsync(MasterModel model)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = @"
UPDATE Master SET
 Username = @Username, TokenNo = @TokenNo, ApplicationNo = @ApplicationNo, ApplicationDate = @ApplicationDate, Place = @Place, ExamFee = @ExamFee, Post = @Post, UnitName = @UnitName,
 FirstName_Marathi = @FirstName_Marathi, FatherName_Marathi = @FatherName_Marathi, Surname_Marathi = @Surname_Marathi, MotherName_Marathi = @MotherName_Marathi,
 FirstName_English = @FirstName_English, FatherName_English = @FatherName_English, Surname_English = @Surname_English, MotherName_English = @MotherName_English,
 Gender = @Gender, DOB = @DOB, Religion = @Religion, Caste = @Caste, SubCaste = @SubCaste,
 Address1 = @Address1, Address2 = @Address2, Address3 = @Address3, Village1 = @Village1, Mukkam_Post = @Mukkam_Post, Taluka = @Taluka, District = @District, State = @State, PinCode = @PinCode,
 PermanantAddress1 = @PermanantAddress1, PermanantAddress2 = @PermanantAddress2, PermanantAddress3 = @PermanantAddress3, PermanantVillage = @PermanantVillage, PermanantMukkam_Post = @PermanantMukkam_Post, PermanantTaluka = @PermanantTaluka, PermanantDistrict = @PermanantDistrict, PermanantState = @PermanantState, PermanantPinCode = @PermanantPinCode,
 EmailID = @EmailID, MobileNo = @MobileNo, ApplicationCategory = @ApplicationCategory, ParallelReservation = @ParallelReservation, FemaleReservation = @FemaleReservation, NonCreamelayer = @NonCreamelayer,
 Maharashtra_Domicile = @Maharashtra_Domicile, MaharashtraDomicileCertNo = @MaharashtraDomicileCertNo, MaharashtraDomicileDate = @MaharashtraDomicileDate,
 Karnataka_Domicile = @Karnataka_Domicile, KarnatakaDomicileCertNo = @KarnatakaDomicileCertNo, KarnatakaDomicileDate = @KarnatakaDomicileDate,
 ExSoldier = @ExSoldier, ExServiceJoiningDate = @ExServiceJoiningDate, ExServiceDependent = @ExServiceDependent, HomeGuard = @HomeGuard, Sportsperson = @Sportsperson, Parttime = @Parttime,
 ParentInPolice = @ParentInPolice, PoliceRank = @PoliceRank, PoliceNatureOfEmployment = @PoliceNatureOfEmployment, PoliceDetails = @PoliceDetails,
 ANATH = @ANATH, AnathDate = @AnathDate, AnathCertificateType = @AnathCertificateType,
 IsNCC = @IsNCC, NCCCertificateNo = @NCCCertificateNo, NCCDate = @NCCDate,
 NaxaliteArea = @NaxaliteArea, SmallVehicle = @SmallVehicle, Prakalpgrast = @Prakalpgrast, Bhukampgrast = @Bhukampgrast, WorkonContract = @WorkonContract,
 IsFarmerSuicide = @IsFarmerSuicide, FarmerSuicideReportNo = @FarmerSuicideReportNo, FarmerSuicideReportDate = @FarmerSuicideReportDate,
 CasteCertificateNo = @CasteCertificateNo, CasteCertificateDate = @CasteCertificateDate,
 SSCBoardName = @SSCBoardName, SSCResult = @SSCResult, SSCMarksObtained = @SSCMarksObtained, SSCTotalMarks = @SSCTotalMarks,
 HSCBoardName = @HSCBoardName, HSCResult = @HSCResult, HSCMarksObtained = @HSCMarksObtained, HSCTotalMarks = @HSCTotalMarks,
 SeventhBoardName = @SeventhBoardName, SeventhResult = @SeventhResult, SeventhMarksObtained = @SeventhMarksObtained, SeventhTotalMarks = @SeventhTotalMarks,
 DiplomaBoardName = @DiplomaBoardName, DiplomaResult = @DiplomaResult, DiplomaMarksObtained = @DiplomaMarksObtained, DiplomaTotalMarks = @DiplomaTotalMarks,
 MSCIT = @MSCIT, GraduationDegree = @GraduationDegree, PostGraduationDegree = @PostGraduationDegree, OtherGraduationDegree = @OtherGraduationDegree, OtherPostGraduationDegree = @OtherPostGraduationDegree,
 UpdatedDate = @UpdatedDate
WHERE SrNo = @SrNo;
";
            using var cmd = new SqlCommand(sql, conn);

            AddParam(cmd, "@Username", model.Username);
            AddParam(cmd, "@TokenNo", model.TokenNo);
            AddParam(cmd, "@ApplicationNo", model.ApplicationNo);
            AddParam(cmd, "@ApplicationDate", model.ApplicationDate);
            AddParam(cmd, "@Place", model.Place);
            AddParam(cmd, "@ExamFee", model.ExamFee);
            AddParam(cmd, "@Post", model.Post);
            AddParam(cmd, "@UnitName", model.UnitName);

            AddParam(cmd, "@FirstName_Marathi", model.FirstName_Marathi);
            AddParam(cmd, "@FatherName_Marathi", model.FatherName_Marathi);
            AddParam(cmd, "@Surname_Marathi", model.Surname_Marathi);
            AddParam(cmd, "@MotherName_Marathi", model.MotherName_Marathi);

            AddParam(cmd, "@FirstName_English", model.FirstName_English);
            AddParam(cmd, "@FatherName_English", model.FatherName_English);
            AddParam(cmd, "@Surname_English", model.Surname_English);
            AddParam(cmd, "@MotherName_English", model.MotherName_English);

            AddParam(cmd, "@Gender", model.Gender);
            AddParam(cmd, "@DOB", model.DOB);
            AddParam(cmd, "@Religion", model.Religion);
            AddParam(cmd, "@Caste", model.Caste);
            AddParam(cmd, "@SubCaste", model.SubCaste);

            AddParam(cmd, "@Address1", model.Address1);
            AddParam(cmd, "@Address2", model.Address2);
            AddParam(cmd, "@Address3", model.Address3);
            AddParam(cmd, "@Village1", model.Village1);
            AddParam(cmd, "@Mukkam_Post", model.Mukkam_Post);
            AddParam(cmd, "@Taluka", model.Taluka);
            AddParam(cmd, "@District", model.District);
            AddParam(cmd, "@State", model.State);
            AddParam(cmd, "@PinCode", model.PinCode);

            AddParam(cmd, "@PermanantAddress1", model.PermanantAddress1);
            AddParam(cmd, "@PermanantAddress2", model.PermanantAddress2);
            AddParam(cmd, "@PermanantAddress3", model.PermanantAddress3);
            AddParam(cmd, "@PermanantVillage", model.PermanantVillage);
            AddParam(cmd, "@PermanantMukkam_Post", model.PermanantMukkam_Post);
            AddParam(cmd, "@PermanantTaluka", model.PermanantTaluka);
            AddParam(cmd, "@PermanantDistrict", model.PermanantDistrict);
            AddParam(cmd, "@PermanantState", model.PermanantState);
            AddParam(cmd, "@PermanantPinCode", model.PermanantPinCode);

            AddParam(cmd, "@EmailID", model.EmailID);
            AddParam(cmd, "@MobileNo", model.MobileNo);
            AddParam(cmd, "@ApplicationCategory", model.ApplicationCategory);
            AddParam(cmd, "@ParallelReservation", model.ParallelReservation);
            AddParam(cmd, "@FemaleReservation", model.FemaleReservation, SqlDbType.Bit);
            AddParam(cmd, "@NonCreamelayer", model.NonCreamelayer, SqlDbType.Bit);

            AddParam(cmd, "@Maharashtra_Domicile", model.Maharashtra_Domicile, SqlDbType.Bit);
            AddParam(cmd, "@MaharashtraDomicileCertNo", model.MaharashtraDomicileCertNo);
            AddParam(cmd, "@MaharashtraDomicileDate", model.MaharashtraDomicileDate);
            AddParam(cmd, "@Karnataka_Domicile", model.Karnataka_Domicile, SqlDbType.Bit);
            AddParam(cmd, "@KarnatakaDomicileCertNo", model.KarnatakaDomicileCertNo);
            AddParam(cmd, "@KarnatakaDomicileDate", model.KarnatakaDomicileDate);

            AddParam(cmd, "@ExSoldier", model.ExSoldier, SqlDbType.Bit);
            AddParam(cmd, "@ExServiceJoiningDate", model.ExServiceJoiningDate);
            AddParam(cmd, "@ExServiceDependent", model.ExServiceDependent, SqlDbType.Bit);
            AddParam(cmd, "@HomeGuard", model.HomeGuard, SqlDbType.Bit);
            AddParam(cmd, "@Sportsperson", model.Sportsperson, SqlDbType.Bit);
            AddParam(cmd, "@Parttime", model.Parttime, SqlDbType.Bit);

            AddParam(cmd, "@ParentInPolice", model.ParentInPolice, SqlDbType.Bit);
            AddParam(cmd, "@PoliceRank", model.PoliceRank);
            AddParam(cmd, "@PoliceNatureOfEmployment", model.PoliceNatureOfEmployment);
            AddParam(cmd, "@PoliceDetails", model.PoliceDetails);

            AddParam(cmd, "@ANATH", model.ANATH, SqlDbType.Bit);
            AddParam(cmd, "@AnathDate", model.AnathDate);
            AddParam(cmd, "@AnathCertificateType", model.AnathCertificateType);

            AddParam(cmd, "@IsNCC", model.IsNCC, SqlDbType.Bit);
            AddParam(cmd, "@NCCCertificateNo", model.NCCCertificateNo);
            AddParam(cmd, "@NCCDate", model.NCCDate);

            AddParam(cmd, "@NaxaliteArea", model.NaxaliteArea, SqlDbType.Bit);
            AddParam(cmd, "@SmallVehicle", model.SmallVehicle, SqlDbType.Bit);
            AddParam(cmd, "@Prakalpgrast", model.Prakalpgrast, SqlDbType.Bit);
            AddParam(cmd, "@Bhukampgrast", model.Bhukampgrast, SqlDbType.Bit);
            AddParam(cmd, "@WorkonContract", model.WorkonContract, SqlDbType.Bit);

            AddParam(cmd, "@IsFarmerSuicide", model.IsFarmerSuicide, SqlDbType.Bit);
            AddParam(cmd, "@FarmerSuicideReportNo", model.FarmerSuicideReportNo);
            AddParam(cmd, "@FarmerSuicideReportDate", model.FarmerSuicideReportDate);

            AddParam(cmd, "@CasteCertificateNo", model.CasteCertificateNo);
            AddParam(cmd, "@CasteCertificateDate", model.CasteCertificateDate);

            AddParam(cmd, "@SSCBoardName", model.SSCBoardName);
            AddParam(cmd, "@SSCResult", model.SSCResult);
            AddParam(cmd, "@SSCMarksObtained", model.SSCMarksObtained);
            AddParam(cmd, "@SSCTotalMarks", model.SSCTotalMarks);

            AddParam(cmd, "@HSCBoardName", model.HSCBoardName);
            AddParam(cmd, "@HSCResult", model.HSCResult);
            AddParam(cmd, "@HSCMarksObtained", model.HSCMarksObtained);
            AddParam(cmd, "@HSCTotalMarks", model.HSCTotalMarks);

            AddParam(cmd, "@SeventhBoardName", model.SeventhBoardName);
            AddParam(cmd, "@SeventhResult", model.SeventhResult);
            AddParam(cmd, "@SeventhMarksObtained", model.SeventhMarksObtained);
            AddParam(cmd, "@SeventhTotalMarks", model.SeventhTotalMarks);

            AddParam(cmd, "@DiplomaBoardName", model.DiplomaBoardName);
            AddParam(cmd, "@DiplomaResult", model.DiplomaResult);
            AddParam(cmd, "@DiplomaMarksObtained", model.DiplomaMarksObtained);
            AddParam(cmd, "@DiplomaTotalMarks", model.DiplomaTotalMarks);

            AddParam(cmd, "@MSCIT", model.MSCIT, SqlDbType.Bit);
            AddParam(cmd, "@GraduationDegree", model.GraduationDegree);
            AddParam(cmd, "@PostGraduationDegree", model.PostGraduationDegree);
            AddParam(cmd, "@OtherGraduationDegree", model.OtherGraduationDegree);
            AddParam(cmd, "@OtherPostGraduationDegree", model.OtherPostGraduationDegree);

            AddParam(cmd, "@UpdatedDate", model.UpdatedDate ?? DateTime.UtcNow);
            AddParam(cmd, "@SrNo", model.SrNo, SqlDbType.Int);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteMasterAsync(int srNo)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = "DELETE FROM Master WHERE SrNo = @SrNo";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@SrNo", srNo, SqlDbType.Int);
            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
        public async Task<MasterModel?> GetMasterByApplicationNoAsync(string applicationNo)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = "SELECT * FROM Master WHERE ApplicationNo = @ApplicationNo";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@ApplicationNo", applicationNo);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapReaderToModel(reader);
            return null;
        }


        private static MasterModel MapReaderToModel(SqlDataReader reader)
        {
            MasterModel m = new MasterModel();

            object GetValue(string name) => reader[name] == DBNull.Value ? null : reader[name];

            m.SrNo = reader["SrNo"] != DBNull.Value ? Convert.ToInt32(reader["SrNo"]) : 0;
            m.Username = GetValue("Username") as string;
            m.TokenNo = GetValue("TokenNo") as string;
            m.ApplicationNo = GetValue("ApplicationNo") as string;
            m.ApplicationDate = GetValue("ApplicationDate") as DateTime?;
            m.Place = GetValue("Place") as string;
            m.ExamFee = GetValue("ExamFee") as decimal?;
            m.Post = GetValue("Post") as string;
            m.UnitName = GetValue("UnitName") as string;

            m.FirstName_Marathi = GetValue("FirstName_Marathi") as string;
            m.FatherName_Marathi = GetValue("FatherName_Marathi") as string;
            m.Surname_Marathi = GetValue("Surname_Marathi") as string;
            m.MotherName_Marathi = GetValue("MotherName_Marathi") as string;

            m.FirstName_English = GetValue("FirstName_English") as string;
            m.FatherName_English = GetValue("FatherName_English") as string;
            m.Surname_English = GetValue("Surname_English") as string;
            m.MotherName_English = GetValue("MotherName_English") as string;

            m.Gender = GetValue("Gender") as string;
            m.DOB = GetValue("DOB") as DateTime?;
            m.Religion = GetValue("Religion") as string;
            m.Caste = GetValue("Caste") as string;
            m.SubCaste = GetValue("SubCaste") as string;

            m.Address1 = GetValue("Address1") as string;
            m.Address2 = GetValue("Address2") as string;
            m.Address3 = GetValue("Address3") as string;
            m.Village1 = GetValue("Village1") as string;
            m.Mukkam_Post = GetValue("Mukkam_Post") as string;
            m.Taluka = GetValue("Taluka") as string;
            m.District = GetValue("District") as string;
            m.State = GetValue("State") as string;
            m.PinCode = GetValue("PinCode") as string;

            m.PermanantAddress1 = GetValue("PermanantAddress1") as string;
            m.PermanantAddress2 = GetValue("PermanantAddress2") as string;
            m.PermanantAddress3 = GetValue("PermanantAddress3") as string;
            m.PermanantVillage = GetValue("PermanantVillage") as string;
            m.PermanantMukkam_Post = GetValue("PermanantMukkam_Post") as string;
            m.PermanantTaluka = GetValue("PermanantTaluka") as string;
            m.PermanantDistrict = GetValue("PermanantDistrict") as string;
            m.PermanantState = GetValue("PermanantState") as string;
            m.PermanantPinCode = GetValue("PermanantPinCode") as string;

            m.EmailID = GetValue("EmailID") as string;
            m.MobileNo = GetValue("MobileNo") as string;
            m.ApplicationCategory = GetValue("ApplicationCategory") as string;
            m.ParallelReservation = GetValue("ParallelReservation") as string;
            m.FemaleReservation = GetValue("FemaleReservation") as bool?;
            m.NonCreamelayer = GetValue("NonCreamelayer") as bool?;

            m.Maharashtra_Domicile = GetValue("Maharashtra_Domicile") as bool?;
            m.MaharashtraDomicileCertNo = GetValue("MaharashtraDomicileCertNo") as string;
            m.MaharashtraDomicileDate = GetValue("MaharashtraDomicileDate") as DateTime?;
            m.Karnataka_Domicile = GetValue("Karnataka_Domicile") as bool?;
            m.KarnatakaDomicileCertNo = GetValue("KarnatakaDomicileCertNo") as string;
            m.KarnatakaDomicileDate = GetValue("KarnatakaDomicileDate") as DateTime?;

            m.ExSoldier = GetValue("ExSoldier") as bool?;
            m.ExServiceJoiningDate = GetValue("ExServiceJoiningDate") as DateTime?;
            m.ExServiceDependent = GetValue("ExServiceDependent") as bool?;
            m.HomeGuard = GetValue("HomeGuard") as bool?;
            m.Sportsperson = GetValue("Sportsperson") as bool?;
            m.Parttime = GetValue("Parttime") as bool?;

            m.ParentInPolice = GetValue("ParentInPolice") as bool?;
            m.PoliceRank = GetValue("PoliceRank") as string;
            m.PoliceNatureOfEmployment = GetValue("PoliceNatureOfEmployment") as string;
            m.PoliceDetails = GetValue("PoliceDetails") as string;

            m.ANATH = GetValue("ANATH") as bool?;
            m.AnathDate = GetValue("AnathDate") as DateTime?;
            m.AnathCertificateType = GetValue("AnathCertificateType") as string;

            m.IsNCC = GetValue("IsNCC") as bool?;
            m.NCCCertificateNo = GetValue("NCCCertificateNo") as string;
            m.NCCDate = GetValue("NCCDate") as DateTime?;

            m.NaxaliteArea = GetValue("NaxaliteArea") as bool?;
            m.SmallVehicle = GetValue("SmallVehicle") as bool?;
            m.Prakalpgrast = GetValue("Prakalpgrast") as bool?;
            m.Bhukampgrast = GetValue("Bhukampgrast") as bool?;
            m.WorkonContract = GetValue("WorkonContract") as bool?;

            m.IsFarmerSuicide = GetValue("IsFarmerSuicide") as bool?;
            m.FarmerSuicideReportNo = GetValue("FarmerSuicideReportNo") as string;
            m.FarmerSuicideReportDate = GetValue("FarmerSuicideReportDate") as DateTime?;

            m.CasteCertificateNo = GetValue("CasteCertificateNo") as string;
            m.CasteCertificateDate = GetValue("CasteCertificateDate") as DateTime?;

            m.SSCBoardName = GetValue("SSCBoardName") as string;
            m.SSCResult = GetValue("SSCResult") as string;
            m.SSCMarksObtained = GetValue("SSCMarksObtained") as decimal?;
            m.SSCTotalMarks = GetValue("SSCTotalMarks") as decimal?;

            m.HSCBoardName = GetValue("HSCBoardName") as string;
            m.HSCResult = GetValue("HSCResult") as string;
            m.HSCMarksObtained = GetValue("HSCMarksObtained") as decimal?;
            m.HSCTotalMarks = GetValue("HSCTotalMarks") as decimal?;

            m.SeventhBoardName = GetValue("SeventhBoardName") as string;
            m.SeventhResult = GetValue("SeventhResult") as string;
            m.SeventhMarksObtained = GetValue("SeventhMarksObtained") as decimal?;
            m.SeventhTotalMarks = GetValue("SeventhTotalMarks") as decimal?;

            m.DiplomaBoardName = GetValue("DiplomaBoardName") as string;
            m.DiplomaResult = GetValue("DiplomaResult") as string;
            m.DiplomaMarksObtained = GetValue("DiplomaMarksObtained") as decimal?;
            m.DiplomaTotalMarks = GetValue("DiplomaTotalMarks") as decimal?;

            m.MSCIT = GetValue("MSCIT") as bool?;
            m.GraduationDegree = GetValue("GraduationDegree") as string;
            m.PostGraduationDegree = GetValue("PostGraduationDegree") as string;
            m.OtherGraduationDegree = GetValue("OtherGraduationDegree") as string;
            m.OtherPostGraduationDegree = GetValue("OtherPostGraduationDegree") as string;

            m.CreatedDate = GetValue("CreatedDate") as DateTime?;
            m.UpdatedDate = GetValue("UpdatedDate") as DateTime?;

            return m;
        }


        // Insert
        public async Task<bool> InsertPetCandidateScoreAsync(PetCandidateScoreModel model)
        {
            using var conn = new SqlConnection(ConnectionString);

            // ✅ Stage logic based on marks_awarded
            string stage = (model.MarksAwarded.HasValue && model.MarksAwarded.Value >= 5) ? "105" : "104";

            var sql = @"
INSERT INTO pet_candidate_score (
    application_no, chest_no, candidate_category, event_name, unit_type,
    start_time, end_time, time_taken_sec, distance_achieved_m, shot_put_weight_kg,
    attempt_no, is_best_attempt, marks_awarded, total_event_marks, remarks, recorded_by, stage
) VALUES (
    @application_no, @chest_no, @candidate_category, @event_name, @unit_type,
    @start_time, @end_time, @time_taken_sec, @distance_achieved_m, @shot_put_weight_kg,
    @attempt_no, @is_best_attempt, @marks_awarded, @total_event_marks, @remarks, @recorded_by, @stage
)";

            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@application_no", model.ApplicationNo);
            AddParam(cmd, "@chest_no", model.ChestNo);
            AddParam(cmd, "@candidate_category", model.CandidateCategory);
            AddParam(cmd, "@event_name", model.EventName);
            AddParam(cmd, "@unit_type", model.UnitType);
            AddParam(cmd, "@start_time", model.StartTime);
            AddParam(cmd, "@end_time", model.EndTime);
            AddParam(cmd, "@time_taken_sec", model.TimeTakenSec);
            AddParam(cmd, "@distance_achieved_m", model.DistanceAchievedM);
            AddParam(cmd, "@shot_put_weight_kg", model.ShotPutWeightKg);
            AddParam(cmd, "@attempt_no", model.AttemptNo);
            AddParam(cmd, "@is_best_attempt", model.IsBestAttempt);
            AddParam(cmd, "@marks_awarded", model.MarksAwarded);
            AddParam(cmd, "@total_event_marks", model.TotalEventMarks);
            AddParam(cmd, "@remarks", model.Remarks);
            AddParam(cmd, "@recorded_by", model.RecordedBy);
            AddParam(cmd, "@stage", stage); // ✅ new parameter

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }


        // Get by ID
        public async Task<PetCandidateScoreModel?> GetPetCandidateScoreByIdAsync(long petId)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = "SELECT * FROM pet_candidate_score WHERE pet_id = @pet_id";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@pet_id", petId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapReaderToPetCandidateScore(reader);
            return null;
        }

        // Get by Application No
        public async Task<PetCandidateScoreModel?> GetPetCandidateScoreByApplicationNoAsync(string applicationNo)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = "SELECT * FROM pet_candidate_score WHERE application_no = @application_no";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@application_no", applicationNo);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapReaderToPetCandidateScore(reader);
            return null;
        }

        // Get by Event
        public async Task<List<PetCandidateScoreModel>> GetPetCandidateScoresByEventAsync(string eventName)
        {
            var list = new List<PetCandidateScoreModel>();
            using var conn = new SqlConnection(ConnectionString);
            var sql = "SELECT * FROM pet_candidate_score WHERE event_name = @event_name";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@event_name", eventName);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapReaderToPetCandidateScore(reader));
            return list;
        }
        public async Task<bool> UpdatePetCandidateScoreAsync(PetCandidateScoreModel model)
        {
            using var conn = new SqlConnection(ConnectionString);

            // ✅ Stage logic based on marks_awarded
            string stage = (model.MarksAwarded.HasValue && model.MarksAwarded.Value >= 5) ? "105" : "104";

            var sql = @"
UPDATE pet_candidate_score SET
    application_no=@application_no,
    chest_no=@chest_no,
    candidate_category=@candidate_category,
    event_name=@event_name,
    unit_type=@unit_type,
    start_time=@start_time,
    end_time=@end_time,
    time_taken_sec=@time_taken_sec,
    distance_achieved_m=@distance_achieved_m,
    shot_put_weight_kg=@shot_put_weight_kg,
    attempt_no=@attempt_no,
    is_best_attempt=@is_best_attempt,
    marks_awarded=@marks_awarded,
    total_event_marks=@total_event_marks,
    remarks=@remarks,
    recorded_by=@recorded_by,
    stage=@stage   -- ✅ added stage update
WHERE pet_id=@pet_id";

            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@pet_id", model.PetId);
            AddParam(cmd, "@application_no", model.ApplicationNo);
            AddParam(cmd, "@chest_no", model.ChestNo);
            AddParam(cmd, "@candidate_category", model.CandidateCategory);
            AddParam(cmd, "@event_name", model.EventName);
            AddParam(cmd, "@unit_type", model.UnitType);
            AddParam(cmd, "@start_time", model.StartTime);
            AddParam(cmd, "@end_time", model.EndTime);
            AddParam(cmd, "@time_taken_sec", model.TimeTakenSec);
            AddParam(cmd, "@distance_achieved_m", model.DistanceAchievedM);
            AddParam(cmd, "@shot_put_weight_kg", model.ShotPutWeightKg);
            AddParam(cmd, "@attempt_no", model.AttemptNo);
            AddParam(cmd, "@is_best_attempt", model.IsBestAttempt);
            AddParam(cmd, "@marks_awarded", model.MarksAwarded);
            AddParam(cmd, "@total_event_marks", model.TotalEventMarks);
            AddParam(cmd, "@remarks", model.Remarks);
            AddParam(cmd, "@recorded_by", model.RecordedBy);
            AddParam(cmd, "@stage", stage); // ✅ new parameter

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        

        // Delete
        public async Task<bool> DeletePetCandidateScoreAsync(long petId)
        {
            using var conn = new SqlConnection(ConnectionString);
            var sql = "DELETE FROM pet_candidate_score WHERE pet_id = @pet_id";
            using var cmd = new SqlCommand(sql, conn);
            AddParam(cmd, "@pet_id", petId);
            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        // Mapper
        private static PetCandidateScoreModel MapReaderToPetCandidateScore(SqlDataReader reader)
        {
            PetCandidateScoreModel m = new PetCandidateScoreModel();

            object GetValue(string name) => reader[name] == DBNull.Value ? null : reader[name];

            m.PetId = Convert.ToInt64(reader["pet_id"]);
            m.ApplicationNo = GetValue("application_no") as string ?? string.Empty;
            m.ChestNo = GetValue("chest_no") as string ?? string.Empty;
            m.CandidateCategory = GetValue("candidate_category") as string ?? string.Empty;
            m.EventName = GetValue("event_name") as string ?? string.Empty;
            m.UnitType = GetValue("unit_type") as string ?? string.Empty;
            m.StartTime = GetValue("start_time") as DateTime?;
            m.EndTime = GetValue("end_time") as DateTime?;
            m.TimeTakenSec = GetValue("time_taken_sec") as decimal?;
            m.DistanceAchievedM = GetValue("distance_achieved_m") as decimal?;
            m.ShotPutWeightKg = GetValue("shot_put_weight_kg") as decimal?;
            m.AttemptNo = GetValue("attempt_no") as int?;
            m.IsBestAttempt = GetValue("is_best_attempt") as string;
            m.MarksAwarded = GetValue("marks_awarded") as int?;
            m.TotalEventMarks = GetValue("total_event_marks") as int?;
            m.Remarks = GetValue("remarks") as string;
            m.RecordedBy = GetValue("recorded_by") as string ?? string.Empty;
            m.RecordedDatetime = GetValue("recorded_datetime") as DateTime?;
           m. Stage = reader["Stage"] as string;


            return m;
        }

    }
}