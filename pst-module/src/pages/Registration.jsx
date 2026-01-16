import { useNavigate } from "react-router-dom";

import React, { useState, useEffect } from "react";
import "../styles/Registration.css";

const API_BASE = "http://localhost:5000/api/Master";

/* ================= HELPERS ================= */
const yesNo = (v) =>
  v === true || v === "YES" ? "YES" : v === false || v === "NO" ? "NO" : "";

const emptyIfNA = (v) =>
  v === null || v === undefined || v === "NA" || v === "0" || v === 0 ? "" : v;

const normalizeDate = (v) => {
  if (!v || v === "01-01-1900") return "";
  if (v.includes("T")) return v.split("T")[0];
  const p = v.split("-");
  return p.length === 3 && p[0].length === 2 ? `${p[2]}-${p[1]}-${p[0]}` : v;
};

const passFail = (v) => (v === "Pass" || v === "Fail" ? v : "");

/* ================= COMPONENT ================= */
export default function Registration() {

  // 🔹 Webcam states
const videoRef = React.useRef(null);
const canvasRef = React.useRef(null);

const [devices, setDevices] = useState([]);
const [selectedDeviceId, setSelectedDeviceId] = useState("");
const [uploadingPhoto, setUploadingPhoto] = useState(false);
const [photoStatus, setPhotoStatus] = useState("");
const [showCamera, setShowCamera] = useState(false);
const [cameraReady, setCameraReady] = useState(false);


  const ITEMS_PER_PAGE = 5;
  const navigate = useNavigate();
  const [applicationNumber, setApplicationNumber] = useState("");
  const [proceed, setProceed] = useState(false);
  const [currentStep, setCurrentStep] = useState(1);
  const [formData, setFormData] = useState({});
  const [readOnlyMode, setReadOnlyMode] = useState(false);
const [applicants, setApplicants] = useState([]);
const [loadingList, setLoadingList] = useState(false);
const [searchTerm, setSearchTerm] = useState("");
const [currentPage, setCurrentPage] = useState(1);


const filtered = applicants.filter((a) => {
  const search = searchTerm.toLowerCase();

  const appNo = (a.applicationNo || "").toLowerCase();
  const engName = `${a.firstName_English || ""} ${a.surname_English || ""}`.toLowerCase();
  const marName = `${a.firstName_Marathi || ""} ${a.surname_Marathi || ""}`.toLowerCase();

  return (
    appNo.includes(search) ||
    engName.includes(search) ||
    marName.includes(search)
  );
});


 /* ================= LOAD MASTER LIST ================= */
  useEffect(() => {
    const loadApplicants = async () => {
      try {
        setLoadingList(true);
        const res = await fetch(`${API_BASE}`);
        if (!res.ok) return;
        const data = await res.json();
        setApplicants(data);
      } catch (err) {
        console.error("Error loading applicants", err);
      } finally {
        setLoadingList(false);
      }
    };
    loadApplicants();
  }, []);
  



  useEffect(() => {
  if (!showCamera) return;

  async function loadDevices() {
    await navigator.mediaDevices.getUserMedia({ video: true });
    const all = await navigator.mediaDevices.enumerateDevices();
    const cams = all.filter(d => d.kind === "videoinput");
    setDevices(cams);
    if (cams.length > 0) setSelectedDeviceId(cams[0].deviceId);
  }
  loadDevices();
}, [showCamera]);

useEffect(() => {
  if (!selectedDeviceId) return;

  async function startCamera() {
    setCameraReady(false);

    if (videoRef.current?.srcObject) {
      videoRef.current.srcObject.getTracks().forEach(t => t.stop());
    }

    const stream = await navigator.mediaDevices.getUserMedia({
      video: { deviceId: { exact: selectedDeviceId } }
    });

    videoRef.current.srcObject = stream;

    // ✅ THIS IS THE FIX
    videoRef.current.onloadedmetadata = () => {
      setCameraReady(true);
    };
  }

  startCamera();
}, [selectedDeviceId]);





const capturePhoto = async () => {
  if (!applicationNumber.trim()) {
    alert("Application Number required");
    return;
  }

  if (!cameraReady) {
    alert("Camera initializing, please wait...");
    return;
  }

  if (uploadingPhoto) return;

  const video = videoRef.current;
  const canvas = canvasRef.current;

  if (!video || !canvas) {
    alert("Camera not ready");
    return;
  }

  canvas.width = video.videoWidth;
  canvas.height = video.videoHeight;

  const ctx = canvas.getContext("2d");
  ctx.drawImage(video, 0, 0);

  canvas.toBlob(async (blob) => {
    if (!blob) {
      alert("Failed to capture image");
      return;
    }

    setUploadingPhoto(true);
    setPhotoStatus("Uploading photo...");

    const formData = new FormData();
    formData.append("file", blob, "photo.png");
    formData.append("applicationNo", applicationNumber);

    try {
      const response = await fetch(
        "http://localhost:5000/api/photo/upload",
        {
          method: "POST",
          body: formData
        }
      );

      if (!response.ok) {
        const errText = await response.text();
        throw new Error(errText);
      }

      setPhotoStatus("✅ Photo stored successfully");
      setShowCamera(false);
    } catch (error) {
      console.error("Upload error:", error);
      setPhotoStatus("❌ Upload failed");
    } finally {
      setUploadingPhoto(false);
    }
  }, "image/png");
};


  /* ================= MAPPER ================= */
  const mapBackendToForm = (d) => ({
    Username: d.username ?? "",
    TokenNo: d.tokenNo ?? "",
    ApplicationNo: d.applicationNo ?? "",
    ApplicationDate: normalizeDate(d.applicationDate),
    Place: d.place ?? "",
    ExamFee: emptyIfNA(d.examFee),
    Post: d.post ?? "",
    UnitName: d.unitName ?? "",

    FirstName_Marathi: d.firstName_Marathi ?? "",
    FatherName_Marathi: d.fatherName_Marathi ?? "",
    Surname_Marathi: d.surname_Marathi ?? "",
    MotherName_Marathi: d.motherName_Marathi ?? "",

    FirstName_English: d.firstName_English ?? "",
    FatherName_English: d.fatherName_English ?? "",
    Surname_English: d.surname_English ?? "",
    MotherName_English: d.motherName_English ?? "",

    Gender: d.gender ?? "",
    DOB: normalizeDate(d.dob),
    Religion: d.religion ?? "",
    Caste: d.caste ?? "",
    SubCaste: d.subCaste ?? "",

    Address1: d.address1 ?? "",
    Address2: d.address2 ?? "",
    Address3: d.address3 ?? "",
    Village1: d.village1 ?? "",
    Mukkam_Post: d.mukkam_Post ?? "",
    Taluka: d.taluka ?? "",
    District: d.district ?? "",
    State: d.state ?? "",
    PinCode: d.pinCode ?? "",

    PermanantAddress1: d.permanantAddress1 ?? "",
    PermanantAddress2: d.permanantAddress2 ?? "",
    PermanantAddress3: d.permanantAddress3 ?? "",
    PermanantVillage: d.permanantVillage ?? "",
    PermanantMukkam_Post: d.permanantMukkam_Post ?? "",
    PermanantTaluka: d.permanantTaluka ?? "",
    PermanantDistrict: d.permanantDistrict ?? "",
    PermanantState: d.permanantState ?? "",
    PermanantPinCode: d.permanantPinCode ?? "",

    EmailID: d.emailID ?? "",
    MobileNo: d.mobileNo ?? "",

    ApplicationCategory: emptyIfNA(d.applicationCategory),
    ParallelReservation: emptyIfNA(d.parallelReservation),
    FemaleReservation: yesNo(d.femaleReservation),
    NonCreamelayer: yesNo(d.nonCreamelayer),

    Maharashtra_Domicile: yesNo(d.maharashtra_Domicile),
    MaharashtraDomicileCertNo: emptyIfNA(d.maharashtraDomicileCertNo),
    MaharashtraDomicileDate: normalizeDate(d.maharashtraDomicileDate),

    Karnataka_Domicile: yesNo(d.karnataka_Domicile),
    KarnatakaDomicileCertNo:
      d.karnatakaDomicileCertNo === null ||
      d.karnatakaDomicileCertNo === undefined
        ? ""
        : d.karnatakaDomicileCertNo,
    KarnatakaDomicileDate: normalizeDate(d.karnatakaDomicileDate),

    ExSoldier: yesNo(d.exSoldier),
    ExServiceJoiningDate: normalizeDate(d.exServiceJoiningDate),
    ExServiceDependent: yesNo(d.exServiceDependent),
    HomeGuard: yesNo(d.homeGuard),
    Sportsperson: yesNo(d.sportsperson),
    Parttime: yesNo(d.parttime),
    ParentInPolice: yesNo(d.parentInPolice),
    PoliceRank: emptyIfNA(d.policeRank),
    PoliceNatureOfEmployment: emptyIfNA(d.policeNatureOfEmployment),
    PoliceDetails: d.policeDetails ?? "",

    ANATH: yesNo(d.anath),
    AnathDate: normalizeDate(d.anathDate),
    AnathCertificateType: emptyIfNA(d.anathCertificateType),

    IsNCC: yesNo(d.isNCC),
    NCCCertificateNo: emptyIfNA(d.nccCertificateNo),
    NCCDate: normalizeDate(d.nccDate),

    NaxaliteArea: yesNo(d.naxaliteArea),
    SmallVehicle: yesNo(d.smallVehicle),

    CasteCertificateNo: emptyIfNA(d.casteCertificateNo),
    CasteCertificateDate: normalizeDate(d.casteCertificateDate),

    SSCBoardName: emptyIfNA(d.sscBoardName),
    SSCResult: passFail(d.sscResult),
    SSCMarksObtained: emptyIfNA(d.sscMarksObtained),
    SSCTotalMarks: emptyIfNA(d.sscTotalMarks),

    HSCBoardName: emptyIfNA(d.hscBoardName),
    HSCResult: passFail(d.hscResult),
    HSCMarksObtained: emptyIfNA(d.hscMarksObtained),
    HSCTotalMarks: emptyIfNA(d.hscTotalMarks),

    SeventhBoardName: emptyIfNA(d.seventhBoardName),
    SeventhResult: passFail(d.seventhResult),
    SeventhMarksObtained: emptyIfNA(d.seventhMarksObtained),
    SeventhTotalMarks: emptyIfNA(d.seventhTotalMarks),

    DiplomaBoardName: emptyIfNA(d.diplomaBoardName),
    DiplomaResult: passFail(d.diplomaResult),
    DiplomaMarksObtained: emptyIfNA(d.diplomaMarksObtained),
    DiplomaTotalMarks: emptyIfNA(d.diplomaTotalMarks),

    MSCIT: yesNo(d.mscit),
    GraduationDegree: emptyIfNA(d.graduationDegree),
    PostGraduationDegree: emptyIfNA(d.postGraduationDegree),
    OtherGraduationDegree: emptyIfNA(d.otherGraduationDegree),
    OtherPostGraduationDegree: emptyIfNA(d.otherPostGraduationDegree),
  });

  /* ================= FETCH ================= */
  const handleProceed = async (e) => {
    e.preventDefault();
    if (!applicationNumber.trim()) return alert("Enter Application No");

    const res = await fetch(`${API_BASE}/by-application/${applicationNumber}`);
    if (!res.ok) return alert("Application not found");

    const data = await res.json();
    setFormData(mapBackendToForm(data));
    setProceed(true);
    setReadOnlyMode(true);
  };

  const handleChange = (e) => {
    if (readOnlyMode) return;
    setFormData((p) => ({ ...p, [e.target.name]: e.target.value }));
  };

  const nextStep = () => setCurrentStep((s) => s + 1);
  const prevStep = () => setCurrentStep((s) => s - 1);

  const handleFinalSubmit = (e) => {
  e.preventDefault();

  console.log("Final Registration Data:", formData);

  // 🔁 redirect to dashboard
  navigate("/");
};

  /* ================= VIEW BUTTON (TABLE) ================= */
  const handleView = async (appNo) => {
    const res = await fetch(`${API_BASE}/by-application/${appNo}`);
    if (!res.ok) return alert("Application not found");

    const data = await res.json();
    setFormData(mapBackendToForm(data));
    setProceed(true);
    setReadOnlyMode(true);
    setCurrentStep(1);
  };


  const Input = ({ name, label, type = "text" }) => (
    <div className="field">
      <label>{label}</label>
      <input
        name={name}
        type={type}
        value={formData[name] || ""}
        onChange={handleChange}
        disabled={readOnlyMode}
      />
    </div>
  );

  const Select = ({ name, label, options }) => (
    <div className="field">
      <label>{label}</label>
      <select
        name={name}
        value={formData[name] || ""}
        onChange={handleChange}
        disabled={readOnlyMode}
      >
        <option value="">Select</option>
        {options.map((o) => (
          <option key={o} value={o}>
            {o}
          </option>
        ))}
      </select>
    </div>
  );

  const YesNo = ({ name, label }) => (
    <Select name={name} label={label} options={["YES", "NO"]} />
  );

    const totalPages = Math.ceil(filtered.length / ITEMS_PER_PAGE);
  const paginatedApplicants  = filtered.slice(
    (currentPage - 1) * ITEMS_PER_PAGE,
    currentPage * ITEMS_PER_PAGE
  );


  /* ================= UI ================= */
  return (
        <div className="registration-form">
      {!proceed ? (
        <>
          {/* ================= APPLICATION NUMBER FORM ================= */}
          <form onSubmit={handleProceed}>
            <h2>Enter Application Number</h2>

            <div className="field">
              <input
                value={applicationNumber}
                onChange={(e) => setApplicationNumber(e.target.value)}
              />
            </div>

            {showCamera && (
  <div style={{ marginTop: "20px" }}>
<video
  ref={videoRef}
  autoPlay
  playsInline
  style={{
    width: "45%",
    margin: "0 auto",
    display: "block"
  }}
/>

    <select
      value={selectedDeviceId}
      onChange={e => setSelectedDeviceId(e.target.value)}
      style={{ width: "100%", marginTop: "10px" }}
    >
      {devices.map((d, i) => (
        <option key={d.deviceId} value={d.deviceId}>
          {d.label || `Camera ${i + 1}`}
        </option>
      ))}
    </select>

<button
  onClick={capturePhoto}
  disabled={!cameraReady || uploadingPhoto}
  className="btn-primary"
>
  {cameraReady ? "Capture Photo" : "Initializing Camera..."}
</button>

    <p style={{ marginTop: "10px" }}>{photoStatus}</p>
  </div>
)}


         {!showCamera && (
  <button
    type="button"
    className="btn-primary"
    disabled={!applicationNumber.trim()}
    onClick={() => setShowCamera(true)}
  >
    Capture Photo
  </button>
)}



          </form>

          {/* ================= TABLE BELOW (NO UI CHANGE) ================= */}
       <div className="applicant-table-wrapper">
  <h3 style={{ marginTop: "30px" }}>Registered Applicants</h3>

  <div className="table-search">
    <input
      type="text"
      placeholder="Search by Application No or Name"
      value={searchTerm}
      onChange={(e) => {
        setSearchTerm(e.target.value);
        setCurrentPage(1); // reset page on search
      }}
    />
  </div>

  {loadingList ? (
    <p>Loading...</p>
  ) : paginatedApplicants.length === 0 ? (
    <div className="no-record-box">
      ❌ No records found
    </div>
  ) : (
    <>
      <table className="applicant-table">
        <thead>
          <tr>
            <th>Application No</th>
            <th>Name (English)</th>
            <th>Name (Marathi)</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {paginatedApplicants.map((a) => (
            <tr key={a.applicationNo}>
              <td>{a.applicationNo}</td>
              <td>{a.firstName_English} {a.surname_English}</td>
              <td>{a.firstName_Marathi} {a.surname_Marathi}</td>
              <td>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={() => handleView(a.applicationNo)}
                >
                  View
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {totalPages > 1 && (
        <div className="pagination-controls">
          <button
            disabled={currentPage === 1}
            onClick={() => setCurrentPage(p => p - 1)}
          >
            Previous
          </button>

          <span>
            Page {currentPage} of {totalPages}
          </span>

          <button
            disabled={currentPage === totalPages}
            onClick={() => setCurrentPage(p => p + 1)}
          >
            Next
          </button>
        </div>
      )}
    </>
  )}
</div>
</>
      ) : (

        /* ========== MAIN REGISTRATION FORM ========== */
<form onSubmit={handleFinalSubmit}>
          {/* ================= STEP 1 ================= */}
          {currentStep === 1 && (
            <>
              <h2>Basic Details</h2>
              <Input name="Username" label="Username" />
              <Input name="TokenNo" label="Token No" />
              <Input name="ApplicationNo" label="Application No" />
              <Input
                name="ApplicationDate"
                label="Application Date"
                type="date"
              />
              <Input name="Place" label="Place" />
              <Input name="ExamFee" label="Exam Fee" />
              <Input name="Post" label="Post" />
              <Input name="UnitName" label="Unit Name" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 2 ================= */}
          {currentStep === 2 && (
            <>
              <h2>Marathi Name</h2>
              <Input name="FirstName_Marathi" label="First Name" />
              <Input name="FatherName_Marathi" label="Father Name" />
              <Input name="Surname_Marathi" label="Surname" />
              <Input name="MotherName_Marathi" label="Mother Name" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 3 ================= */}
          {currentStep === 3 && (
            <>
              <h2>English Name</h2>
              <Input name="FirstName_English" label="First Name" />
              <Input name="FatherName_English" label="Father Name" />
              <Input name="Surname_English" label="Surname" />
              <Input name="MotherName_English" label="Mother Name" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 4 ================= */}
          {currentStep === 4 && (
            <>
              <h2>Personal Details</h2>
              <Select
                name="Gender"
                label="Gender"
                options={["MALE", "FEMALE", "OTHER"]}
              />
              <Input name="DOB" label="DOB" type="date" />
              <Input name="Religion" label="Religion" />
              <Input name="Caste" label="Caste" />
              <Input name="SubCaste" label="Sub Caste" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 5 ================= */}
          {currentStep === 5 && (
            <>
              <h2>Address</h2>
              <Input name="Address1" label="Address 1" />
              <Input name="Address2" label="Address 2" />
              <Input name="Address3" label="Address 3" />
              <Input name="Village1" label="Village" />
              <Input name="Mukkam_Post" label="Mukkam / Post" />
              <Input name="Taluka" label="Taluka" />
              <Input name="District" label="District" />
              <Input name="State" label="State" />
              <Input name="PinCode" label="Pin Code" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 6 ================= */}
          {currentStep === 6 && (
            <>
              <h2>Education & Computer</h2>
              <YesNo name="MSCIT" label="MS-CIT Completed" />
              <Input name="GraduationDegree" label="Graduation Degree" />
              <Input
                name="PostGraduationDegree"
                label="Post Graduation Degree"
              />
              <Input
                name="OtherGraduationDegree"
                label="Other Graduation Degree"
              />
              <Input
                name="OtherPostGraduationDegree"
                label="Other Post Graduation Degree"
              />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {currentStep === 7 && (
            <>
              <h2>Contact Details</h2>
              <Input name="EmailID" label="Email ID" />
              <Input name="MobileNo" label="Mobile No" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {currentStep === 8 && (
            <>
              <h2>Category & Reservation</h2>
              <Input name="ApplicationCategory" label="Application Category" />
              <Input name="ParallelReservation" label="Parallel Reservation" />
              <YesNo name="FemaleReservation" label="Female Reservation" />
              <YesNo name="NonCreamelayer" label="Non Creamy Layer" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {currentStep === 9 && (
            <>
              <h2>Domicile Details</h2>
              <YesNo name="Maharashtra_Domicile" label="Maharashtra Domicile" />
              <Input name="MaharashtraDomicileCertNo" label="Certificate No" />
              <Input name="MaharashtraDomicileDate" label="Date" type="date" />

              <YesNo name="Karnataka_Domicile" label="Karnataka Domicile" />
              <Input name="KarnatakaDomicileCertNo" label="Certificate No" />
              <Input name="KarnatakaDomicileDate" label="Date" type="date" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {currentStep === 10 && (
            <>
              <h2>Service / Reservation</h2>
              <YesNo name="ExSoldier" label="Ex-Soldier" />
              <Input
                name="ExServiceJoiningDate"
                label="Joining Date"
                type="date"
              />
              <YesNo name="ExServiceDependent" label="Ex-Service Dependent" />
              <YesNo name="HomeGuard" label="Home Guard" />
              <YesNo name="Sportsperson" label="Sportsperson" />
              <YesNo name="Parttime" label="Part Time" />
              <YesNo name="ParentInPolice" label="Parent in Police" />

              <Input name="PoliceRank" label="Police Rank" />
              <Input
                name="PoliceNatureOfEmployment"
                label="Nature of Employment"
              />
              <Input name="PoliceDetails" label="Police Details" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {currentStep === 11 && (
            <>
              <h2>Special Categories</h2>
              <YesNo name="ANATH" label="Orphan (ANATH)" />
              <Input name="AnathDate" label="Date" type="date" />
              <Input name="AnathCertificateType" label="Certificate Type" />

              <YesNo name="IsNCC" label="NCC Cadet" />
              <Input name="NCCCertificateNo" label="Certificate No" />
              <Input name="NCCDate" label="Date" type="date" />

              <YesNo name="NaxaliteArea" label="Naxalite Area" />
              <YesNo name="SmallVehicle" label="Small Vehicle License" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 12 ================= */}
          {currentStep === 12 && (
            <>
              <h2>Caste Certificate</h2>
              <Input name="CasteCertificateNo" label="Certificate No" />
              <Input name="CasteCertificateDate" label="Date" type="date" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 13 ================= */}
          {currentStep === 13 && (
            <>
              <h2>SSC Details</h2>
              <Input name="SSCBoardName" label="Board Name" />
              <Select
                name="SSCResult"
                label="Result"
                options={["Pass", "Fail"]}
              />
              <Input name="SSCMarksObtained" label="Marks Obtained" />
              <Input name="SSCTotalMarks" label="Total Marks" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 14 ================= */}
          {currentStep === 14 && (
            <>
              <h2>HSC Details</h2>
              <Input name="HSCBoardName" label="Board Name" />
              <Select
                name="HSCResult"
                label="Result"
                options={["Pass", "Fail"]}
              />
              <Input name="HSCMarksObtained" label="Marks Obtained" />
              <Input name="HSCTotalMarks" label="Total Marks" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 15 ================= */}
          {currentStep === 15 && (
            <>
              <h2>Seventh Standard</h2>
              <Input name="SeventhBoardName" label="Board Name" />
              <Select
                name="SeventhResult"
                label="Result"
                options={["Pass", "Fail"]}
              />
              <Input name="SeventhMarksObtained" label="Marks Obtained" />
              <Input name="SeventhTotalMarks" label="Total Marks" />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button
                  type="button"
                  className="btn-primary"
                  onClick={nextStep}
                >
                  Next
                </button>
              </div>
            </>
          )}

          {/* ================= STEP 16 ================= */}
          {currentStep === 16 && (
            <>
              <h2>Diploma & Final Education</h2>

              <Input name="DiplomaBoardName" label="Diploma Board Name" />
              <Select
                name="DiplomaResult"
                label="Result"
                options={["Pass", "Fail"]}
              />
              <Input name="DiplomaMarksObtained" label="Marks Obtained" />
              <Input name="DiplomaTotalMarks" label="Total Marks" />

              <YesNo name="MSCIT" label="MS-CIT Completed" />
              <Input name="GraduationDegree" label="Graduation Degree" />
              <Input
                name="PostGraduationDegree"
                label="Post Graduation Degree"
              />
              <Input
                name="OtherGraduationDegree"
                label="Other Graduation Degree"
              />
              <Input
                name="OtherPostGraduationDegree"
                label="Other Post Graduation Degree"
              />

              <div className="actions">
                <button
                  type="button"
                  className="btn-primary"
                  onClick={prevStep}
                >
                  Back
                </button>
                <button type="submit" className="btn-primary">
                  SUBMIT
                </button>
                
              </div>
            </>
          )}
        </form>
      )}
            <canvas ref={canvasRef} style={{ display: "none" }} />

    </div>
  );
}
