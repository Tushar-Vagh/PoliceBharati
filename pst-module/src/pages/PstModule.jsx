import { useParams } from "react-router-dom";
import React, { useState, useEffect, useRef } from "react";
import { evaluatePST } from "../utils/grRules";
import "../styles/pst.css";
import { useReactToPrint } from "react-to-print";
import AdmissionCard from "../components/AdmissionCard";
import RejectionSlip from "../components/RejectionSlip";
import Toast from "../components/Toast";

const MASTER_API = "/api/Master";
const PST_API = "/api/PhysicalStandard";

const PstModule = () => {
  const [formData, setFormData] = useState({
    pst_id: "",
    application_number: "",
    gender: "",
    height_cm: "",
    chest_cm: "",
    weight_kg: "",
    measured_by_officer_name: "",
    remarks: "",
  });

  const MIN_APP_NO_LENGTH = 11;

  const [errorDialog, setErrorDialog] = useState({
    open: false,
    message: "",
  });

  const { applicationNo } = useParams();
  const ITEMS_PER_PAGE = 2;
  const [currentPage, setCurrentPage] = useState(1);

  const [record, setRecord] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showAdmissionCard, setShowAdmissionCard] = useState(false);

  const [applicants, setApplicants] = useState([]);
  const [loadingList, setLoadingList] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");

  const componentRef = useRef(null);

  const [toast, setToast] = useState({
    visible: false,
    message: "",
    type: "info",
  });

  const handleShowToast = (message, type = "info") =>
    setToast({ visible: true, message, type });

  const handlePrint = useReactToPrint({
    contentRef: componentRef,
    documentTitle: "Admission Card",
  });

  /* ================= INITIAL LOAD ================= */
  useEffect(() => {
    setIsLoading(false);
  }, []);

  useEffect(() => {
    const loadApplicants = async () => {
      try {
        setLoadingList(true);
        const res = await fetch(MASTER_API);
        if (!res.ok) return;
        const data = await res.json();
        setApplicants(data);
      } catch (err) {
        console.error("Failed to load applicants", err);
      } finally {
        setLoadingList(false);
      }
    };

    loadApplicants();
  }, []);

  useEffect(() => {
    setCurrentPage(1);
  }, [searchTerm]);

  const filteredApplicants = applicants.filter((a) => {
    const s = searchTerm.toLowerCase();

    const appNo = (a.applicationNo || "").toLowerCase();
    const engName = `${a.firstName_English || ""} ${
      a.surname_English || ""
    }`.toLowerCase();
    const marName = `${a.firstName_Marathi || ""} ${
      a.surname_Marathi || ""
    }`.toLowerCase();

    return appNo.includes(s) || engName.includes(s) || marName.includes(s);
  });

  useEffect(() => {
    if (applicationNo) {
      setFormData((prev) => ({
        ...prev,
        application_number: applicationNo,
      }));
      fetchGenderByApplication(applicationNo);
    }
  }, [applicationNo]);

  const totalPages = Math.ceil(filteredApplicants.length / ITEMS_PER_PAGE);

  const paginatedApplicants = filteredApplicants.slice(
    (currentPage - 1) * ITEMS_PER_PAGE,
    currentPage * ITEMS_PER_PAGE
  );

  const handleSelectCandidate = (appNo) => {
    setFormData((prev) => ({
      ...prev,
      application_number: appNo,
    }));

    fetchGenderByApplication(appNo);
  };

  /* ================= FETCH GENDER FROM MASTER ================= */
  const fetchGenderByApplication = async (appNo) => {
    if (!appNo) return;

    try {
      const res = await fetch(`${MASTER_API}/by-application/${appNo}`);
      if (!res.ok) {
        setErrorDialog({
          open: true,
          message: "Application number not found in Master records.",
        });
        setFormData((prev) => ({ ...prev, gender: "" }));
        return;
      }

      const data = await res.json();
      setFormData((prev) => ({
        ...prev,
        gender: data.gender || "Male",
      }));
    } catch {
      setErrorDialog({
        open: true,
        message: "Unable to fetch application details. Please try again.",
      });
    }
  };

  /* ================= INPUT CHANGE ================= */
  const handleChange = (e) => {
    const { name, value } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));

    if (name === "application_number") {
      setErrorDialog({ open: false, message: "" });

      // ⛔ Do not call API while typing short input
      if (value.length < MIN_APP_NO_LENGTH) {
        setFormData((prev) => ({ ...prev, gender: "" }));
        return;
      }

      fetchGenderByApplication(value.trim());
    }
  };

  /* ================= SUBMIT PST ================= */
  const handleSubmit = async (e) => {
    e.preventDefault();

    const evaluation = evaluatePST(formData);

    const now = new Date();
    const formattedDateTime =
      `${String(now.getDate()).padStart(2, "0")}-${String(
        now.getMonth() + 1
      ).padStart(2, "0")}-${now.getFullYear()} ` +
      `${now.getHours() % 12 || 12}:${String(now.getMinutes()).padStart(
        2,
        "0"
      )} ` +
      `${now.getHours() >= 12 ? "PM" : "AM"}`;

    const receiptNumber = `REC-${Date.now().toString().slice(-6)}`;

    /* ---- SAVE TO DB ---- */
    const payload = {
      applicationNumber: formData.application_number,
      heightCm: Number(formData.height_cm),
      chestCm: formData.chest_cm ? Number(formData.chest_cm) : null,
      weightKg: Number(formData.weight_kg),
      gender: formData.gender,
      measuredByOfficerId: formData.measured_by_officer_name,
      remarks: formData.remarks,
      receiptNumber: receiptNumber,
    };

    try {
      const res = await fetch(PST_API, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        handleShowToast("Failed to save PST record", "error");
        return;
      }

      /* ---- SHOW RECEIPT ---- */
      setRecord({
        ...formData,
        pst_status: evaluation.status,
        rejection_reasons: evaluation.reasons,
        measurement_date_time: formattedDateTime,
        receipt_number: receiptNumber,
      });
    } catch {
      handleShowToast("Server error while saving PST", "error");
    }
  };

  const handleNextCandidate = () => {
    setRecord(null);
    setShowAdmissionCard(false);
    setFormData((prev) => ({
      pst_id: "",
      application_number: "",
      gender: "",
      height_cm: "",
      chest_cm: "",
      weight_kg: "",
      measured_by_officer_name: "",
      remarks: "",
    }));
  };

  if (isLoading) return <div className="loader">Loading System...</div>;

  return (
    <div className="pst-full-page">
      {!record ? (
        <form onSubmit={handleSubmit} className="bank-ledger-form no-print">
          <div className="ledger-grid">
            <div className="ledger-row">
              <label>App No</label>
              <input
                name="application_number"
                value={formData.application_number}
                onChange={handleChange}
                required
              />
            </div>

            {/* ================= ERROR DIALOG ================= */}
            {errorDialog.open && (
              <div className="pst-dialog-overlay">
                <div className="pst-dialog-box">
                  <h3>Error</h3>
                  <p>{errorDialog.message}</p>
                  <button
                    className="btn-secondary"
                    onClick={() => setErrorDialog({ open: false, message: "" })}
                  >
                    OK
                  </button>
                </div>
              </div>
            )}

            <div className="ledger-row">
              <label>Gender</label>
              <input
                name="gender"
                value={formData.gender}
                readOnly
                className="readonly-field"
              />

              <label>Officer Name</label>
              <input
                name="measured_by_officer_name"
                value={formData.measured_by_officer_name}
                onChange={handleChange}
                required
              />
            </div>

            <div className="ledger-row">
              <label>Height (cm)</label>
              <input
                type="number"
                step="0.1"
                name="height_cm"
                value={formData.height_cm}
                onChange={handleChange}
                required
              />

              <label>Chest (cm)</label>
              <input
                type="number"
                step="0.1"
                name="chest_cm"
                value={formData.chest_cm}
                onChange={handleChange}
                required
              />
            </div>

            <div className="ledger-row">
              <label>Weight (kg)</label>
              <input
                type="number"
                step="0.1"
                name="weight_kg"
                value={formData.weight_kg}
                onChange={handleChange}
                required
              />

              <label>Remarks</label>
              <input
                name="remarks"
                value={formData.remarks}
                onChange={handleChange}
                style={{ flexGrow: 1 }}
              />
            </div>
          </div>

          <div
            className="form-footer"
            style={{ display: "flex", justifyContent: "center" }}
          >
            <button type="submit" className="btn-process">
              Process Evaluation & Print
            </button>
          </div>

        
        </form>
      ) : (
        !showAdmissionCard && (
          <div className="receipt-area">
            {record.pst_status === "Pass" ? (
              <div className="official-receipt">
                <div className="receipt-header">
                  <h2>PST EVALUATION RECEIPT</h2>
                  <div className="receipt-meta">
                    <span>
                      <strong>Receipt No:</strong> {record.receipt_number}
                    </span>
                    <span>
                      <strong>Date:</strong> {record.measurement_date_time}
                    </span>
                  </div>
                </div>
                <div className="status-banner pass">RESULT: PASS</div>
              </div>
            ) : (
              <RejectionSlip
                ref={componentRef}
                candidate={record}
                reasons={record.rejection_reasons}
              />
            )}

            <div className="action-buttons no-print">
              <button
                onClick={() =>
                  record.pst_status === "Pass"
                    ? setShowAdmissionCard(true)
                    : handlePrint()
                }
                className="btn-print"
              >
                {record.pst_status === "Pass"
                  ? "Print Admission Card"
                  : "Print Rejection Slip"}
              </button>

              <button onClick={handleNextCandidate} className="btn-secondary">
                Next Candidate
              </button>
            </div>
          </div>
        )
      )}

      {/* ================= MODAL ================= */}
      {showAdmissionCard && (
        <div className="pst-modal">
          <div className="pst-modal-content">
            <div className="pst-modal-header">
              <h3>Admission Card Preview</h3>
              <div>
                <button onClick={handlePrint} className="btn-print">
                  Print
                </button>
                <button
                  onClick={() => setShowAdmissionCard(false)}
                  className="btn-secondary"
                >
                  Back
                </button>
              </div>
            </div>

            <AdmissionCard ref={componentRef} candidate={record} />
          </div>
        </div>
      )}

      {toast.visible && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast({ ...toast, visible: false })}
        />
      )}
    </div>
  );
};

export default PstModule;
