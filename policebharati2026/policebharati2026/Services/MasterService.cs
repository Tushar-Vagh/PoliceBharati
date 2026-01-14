using System.Collections.Generic;
using System.Threading.Tasks;
using policebharati2026.Data;
using MasterApi.DTOs;
using MasterApi.Models;

namespace MasterApi.Services
{
    public class MasterService
    {
        private readonly SqlHelper _sqlHelper;

        public MasterService(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<bool> AddMasterAsync(MasterDto dto)
        {
            var model = MapDtoToModel(dto);
            return await _sqlHelper.InsertMasterAsync(model);
        }

        public async Task<List<MasterModel>> GetAllAsync()
        {
            return await _sqlHelper.GetAllMastersAsync();
        }

        public async Task<MasterModel?> GetByIdAsync(int srNo)
        {
            return await _sqlHelper.GetMasterByIdAsync(srNo);
        }

        public async Task<bool> UpdateAsync(int srNo, MasterDto dto)
        {
            var model = MapDtoToModel(dto);
            model.SrNo = srNo;
            model.UpdatedDate = dto.UpdatedDate ?? System.DateTime.UtcNow;
            return await _sqlHelper.UpdateMasterAsync(model);
        }

        public async Task<bool> DeleteAsync(int srNo)
        {
            return await _sqlHelper.DeleteMasterAsync(srNo);
        }
        public async Task<MasterModel?> GetByApplicationNoAsync(string applicationNo)
        {
            return await _sqlHelper.GetMasterByApplicationNoAsync(applicationNo);
        }

        private MasterModel MapDtoToModel(MasterDto dto)
        {
            return new MasterModel
            {
                Username = dto.Username,
                TokenNo = dto.TokenNo,
                ApplicationNo = dto.ApplicationNo,
                ApplicationDate = dto.ApplicationDate,
                Place = dto.Place,
                ExamFee = dto.ExamFee,
                Post = dto.Post,
                UnitName = dto.UnitName,

                FirstName_Marathi = dto.FirstName_Marathi,
                FatherName_Marathi = dto.FatherName_Marathi,
                Surname_Marathi = dto.Surname_Marathi,
                MotherName_Marathi = dto.MotherName_Marathi,

                FirstName_English = dto.FirstName_English,
                FatherName_English = dto.FatherName_English,
                Surname_English = dto.Surname_English,
                MotherName_English = dto.MotherName_English,

                Gender = dto.Gender,
                DOB = dto.DOB,
                Religion = dto.Religion,
                Caste = dto.Caste,
                SubCaste = dto.SubCaste,

                Address1 = dto.Address1,
                Address2 = dto.Address2,
                Address3 = dto.Address3,
                Village1 = dto.Village1,
                Mukkam_Post = dto.Mukkam_Post,
                Taluka = dto.Taluka,
                District = dto.District,
                State = dto.State,
                PinCode = dto.PinCode,

                PermanantAddress1 = dto.PermanantAddress1,
                PermanantAddress2 = dto.PermanantAddress2,
                PermanantAddress3 = dto.PermanantAddress3,
                PermanantVillage = dto.PermanantVillage,
                PermanantMukkam_Post = dto.PermanantMukkam_Post,
                PermanantTaluka = dto.PermanantTaluka,
                PermanantDistrict = dto.PermanantDistrict,
                PermanantState = dto.PermanantState,
                PermanantPinCode = dto.PermanantPinCode,

                EmailID = dto.EmailID,
                MobileNo = dto.MobileNo,
                ApplicationCategory = dto.ApplicationCategory,
                ParallelReservation = dto.ParallelReservation,
                FemaleReservation = dto.FemaleReservation,
                NonCreamelayer = dto.NonCreamelayer,

                Maharashtra_Domicile = dto.Maharashtra_Domicile,
                MaharashtraDomicileCertNo = dto.MaharashtraDomicileCertNo,
                MaharashtraDomicileDate = dto.MaharashtraDomicileDate,
                Karnataka_Domicile = dto.Karnataka_Domicile,
                KarnatakaDomicileCertNo = dto.KarnatakaDomicileCertNo,
                KarnatakaDomicileDate = dto.KarnatakaDomicileDate,

                ExSoldier = dto.ExSoldier,
                ExServiceJoiningDate = dto.ExServiceJoiningDate,
                ExServiceDependent = dto.ExServiceDependent,
                HomeGuard = dto.HomeGuard,
                Sportsperson = dto.Sportsperson,
                Parttime = dto.Parttime,

                ParentInPolice = dto.ParentInPolice,
                PoliceRank = dto.PoliceRank,
                PoliceNatureOfEmployment = dto.PoliceNatureOfEmployment,
                PoliceDetails = dto.PoliceDetails,

                ANATH = dto.ANATH,
                AnathDate = dto.AnathDate,
                AnathCertificateType = dto.AnathCertificateType,

                IsNCC = dto.IsNCC,
                NCCCertificateNo = dto.NCCCertificateNo,
                NCCDate = dto.NCCDate,

                NaxaliteArea = dto.NaxaliteArea,
                SmallVehicle = dto.SmallVehicle,
                Prakalpgrast = dto.Prakalpgrast,
                Bhukampgrast = dto.Bhukampgrast,
                WorkonContract = dto.WorkonContract,

                IsFarmerSuicide = dto.IsFarmerSuicide,
                FarmerSuicideReportNo = dto.FarmerSuicideReportNo,
                FarmerSuicideReportDate = dto.FarmerSuicideReportDate,

                CasteCertificateNo = dto.CasteCertificateNo,
                CasteCertificateDate = dto.CasteCertificateDate,

                SSCBoardName = dto.SSCBoardName,
                SSCResult = dto.SSCResult,
                SSCMarksObtained = dto.SSCMarksObtained, 
                SSCTotalMarks = dto.SSCTotalMarks,

                HSCBoardName = dto.HSCBoardName,
                HSCResult = dto.HSCResult,
                HSCMarksObtained = dto.HSCMarksObtained,
                HSCTotalMarks = dto.HSCTotalMarks,

                SeventhBoardName = dto.SeventhBoardName,
                SeventhResult = dto.SeventhResult,
                SeventhMarksObtained = dto.SeventhMarksObtained,
                SeventhTotalMarks = dto.SeventhTotalMarks,

                DiplomaBoardName = dto.DiplomaBoardName,
                DiplomaResult = dto.DiplomaResult,
                DiplomaMarksObtained = dto.DiplomaMarksObtained,
                DiplomaTotalMarks = dto.DiplomaTotalMarks,

                MSCIT = dto.MSCIT,
                GraduationDegree = dto.GraduationDegree,
                PostGraduationDegree = dto.PostGraduationDegree,
                OtherGraduationDegree = dto.OtherGraduationDegree,
                OtherPostGraduationDegree = dto.OtherPostGraduationDegree,

                CreatedDate = dto.CreatedDate ?? System.DateTime.UtcNow,
                UpdatedDate = dto.UpdatedDate ?? System.DateTime.UtcNow
            };
        }
    }
}
