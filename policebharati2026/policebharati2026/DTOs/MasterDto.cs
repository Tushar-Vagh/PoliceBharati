using System;

namespace MasterApi.DTOs
{
    public class MasterDto
    {
        public string? Username { get; set; }
        public string? TokenNo { get; set; }
        public string? ApplicationNo { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? Place { get; set; }
        public decimal? ExamFee { get; set; }
        public string? Post { get; set; }
        public string? UnitName { get; set; }

        public string? FirstName_Marathi { get; set; }
        public string? FatherName_Marathi { get; set; }
        public string? Surname_Marathi { get; set; }
        public string? MotherName_Marathi { get; set; }

        public string? FirstName_English { get; set; }
        public string? FatherName_English { get; set; }
        public string? Surname_English { get; set; }
        public string? MotherName_English { get; set; }

        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? SubCaste { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Village1 { get; set; }
        public string? Mukkam_Post { get; set; }
        public string? Taluka { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }

        public string? PermanantAddress1 { get; set; }
        public string? PermanantAddress2 { get; set; }
        public string? PermanantAddress3 { get; set; }
        public string? PermanantVillage { get; set; }
        public string? PermanantMukkam_Post { get; set; }
        public string? PermanantTaluka { get; set; }
        public string? PermanantDistrict { get; set; }
        public string? PermanantState { get; set; }
        public string? PermanantPinCode { get; set; }

        public string? EmailID { get; set; }
        public string? MobileNo { get; set; }
        public string? ApplicationCategory { get; set; }
        public string? ParallelReservation { get; set; }
        public bool? FemaleReservation { get; set; }
        public bool? NonCreamelayer { get; set; }

        public bool? Maharashtra_Domicile { get; set; }
        public string? MaharashtraDomicileCertNo { get; set; }
        public DateTime? MaharashtraDomicileDate { get; set; }
        public bool? Karnataka_Domicile { get; set; }
        public string? KarnatakaDomicileCertNo { get; set; }
        public DateTime? KarnatakaDomicileDate { get; set; }

        public bool? ExSoldier { get; set; }
        public DateTime? ExServiceJoiningDate { get; set; }
        public bool? ExServiceDependent { get; set; }
        public bool? HomeGuard { get; set; }
        public bool? Sportsperson { get; set; }
        public bool? Parttime { get; set; }
        public bool? ParentInPolice { get; set; }
        public string? PoliceRank { get; set; }
        public string? PoliceNatureOfEmployment { get; set; }
        public string? PoliceDetails { get; set; }

        public bool? ANATH { get; set; }
        public DateTime? AnathDate { get; set; }
        public string? AnathCertificateType { get; set; }

        public bool? IsNCC { get; set; }
        public string? NCCCertificateNo { get; set; }
        public DateTime? NCCDate { get; set; }

        public bool? NaxaliteArea { get; set; }
        public bool? SmallVehicle { get; set; }
        public bool? Prakalpgrast { get; set; }
        public bool? Bhukampgrast { get; set; }
        public bool? WorkonContract { get; set; }
        public bool? IsFarmerSuicide { get; set; }
        public string? FarmerSuicideReportNo { get; set; }
        public DateTime? FarmerSuicideReportDate { get; set; }

        public string? CasteCertificateNo { get; set; }
        public DateTime? CasteCertificateDate { get; set; }

        public string? SSCBoardName { get; set; }
        public string? SSCResult { get; set; }
        public decimal? SSCMarksObtained { get; set; }
        public decimal? SSCTotalMarks { get; set; }

        public string? HSCBoardName { get; set; }
        public string? HSCResult { get; set; }
        public decimal? HSCMarksObtained { get; set; }
        public decimal? HSCTotalMarks { get; set; }

        public string? SeventhBoardName { get; set; }
        public string? SeventhResult { get; set; }
        public decimal? SeventhMarksObtained { get; set; }
        public decimal? SeventhTotalMarks { get; set; }

        public string? DiplomaBoardName { get; set; }
        public string? DiplomaResult { get; set; }
        public decimal? DiplomaMarksObtained { get; set; }
        public decimal? DiplomaTotalMarks { get; set; }

        public bool? MSCIT { get; set; }
        public string? GraduationDegree { get; set; }
        public string? PostGraduationDegree { get; set; }
        public string? OtherGraduationDegree { get; set; }
        public string? OtherPostGraduationDegree { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
