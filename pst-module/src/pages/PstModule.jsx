import { useParams } from "react-router-dom";
import React, { useState, useEffect, useRef } from "react";
import { evaluatePST } from "../utils/grRules";
import "../styles/pst.css";
import { useReactToPrint } from "react-to-print";
import AdmissionCard from "../components/AdmissionCard";
import RejectionSlip from "../components/RejectionSlip";
import Toast from "../components/Toast";

const MASTER_API = "http://localhost:5000/api/Master";
const PST_API = "http://localhost:5000/api/PhysicalStandard";

const PstModule = () => {
  const { applicationNo } = useParams();

  const [formData, setFormData] = useState({
    application_number: "",
    gender: "",
    height_cm: "",
    chest_cm: "",
    weight_kg: "",
    remarks: "",
  });

  // ✅ DB-driven completion state ONLY
  const [completion, setCompletion] = useState({
    heightDone: false,
    weightDone: false,
    chestDone: false,
  });

  const [record, setRecord] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showAdmissionCard, setShowAdmissionCard] = useState(false);

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
  documentTitle: "PST_Print",
  pageStyle: `
    @page {
      size: A4;
      margin: 8mm;
    }

    html, body {
      width: 210mm;
      height: 297mm;
      margin: 0;
      padding: 0;
    }

    * {
      box-sizing: border-box;
      page-break-inside: avoid !important;
      page-break-after: avoid !important;
      page-break-before: avoid !important;
    }

    /* 🔹 SHRINK TEXT ONLY FOR PRINT */
    body {
      font-size: 25px !important;
      line-height: 1.2 !important;
    }

    h1, h2 {
      font-size: 16px !important;
    }

    h3 {
      font-size: 14px !important;
    }

    table td, table th, p, span, div {
      font-size: 19px !important;
    }

    .no-print, button {
      display: none !important;
    }
  `,
});


  /* ================= INITIAL LOAD ================= */
  useEffect(() => {
    setIsLoading(false);
  }, []);

  /* ================= FETCH GENDER ================= */
  const fetchGenderByApplication = async (appNo) => {
    if (!appNo) return;

    try {
      const res = await fetch(`${MASTER_API}/by-application/${appNo}`);
      if (!res.ok) {
        setFormData((prev) => ({ ...prev, gender: "" }));
        return;
      }
      const data = await res.json();
      setFormData((prev) => ({
        ...prev,
        gender: data.gender || "Male",
      }));
    } catch {}
  };

  /* ================= FETCH STATUS (DB IS TRUTH) ================= */
  useEffect(() => {
    if (!applicationNo) {
      setCompletion({ heightDone: false, weightDone: false, chestDone: false });
      return;
    }

    // reset first
    setCompletion({ heightDone: false, weightDone: false, chestDone: false });

    fetch(`${PST_API}/status/${applicationNo}`)
      .then((res) => (res.ok ? res.json() : null))
      .then((data) => {
        if (!data) return;
        setCompletion({
          heightDone: data.heightDone === true,
          weightDone: data.weightDone === true,
          chestDone: data.chestDone === true,
        });
      });
  }, [applicationNo]);

  /* ================= LOAD APPLICATION ================= */
  useEffect(() => {
    if (!applicationNo) return;
    setFormData((prev) => ({ ...prev, application_number: applicationNo }));
    fetchGenderByApplication(applicationNo);
  }, [applicationNo]);

  /* ================= INPUT CHANGE ================= */
const MIN_APP_NO_LENGTH = 11;

const handleChange = (e) => {
  const { name, value } = e.target;

  setFormData((prev) => ({
    ...prev,
    [name]: value,
  }));

  // 🔑 Only for Application No
  if (name === "application_number") {
    // reset DB-driven state while typing new app no
    setCompletion({
      heightDone: false,
      weightDone: false,
      chestDone: false,
    });

    setRecord(null); // reset previous receipt

    // wait until full app no entered
    if (value.length < MIN_APP_NO_LENGTH) {
      setFormData((prev) => ({ ...prev, gender: "" }));
      return;
    }

    // ✅ fetch gender from Master
    fetchGenderByApplication(value.trim());

    // ✅ fetch measurement status from DB
    fetch(`${PST_API}/status/${value.trim()}`)
      .then((res) => (res.ok ? res.json() : null))
      .then((data) => {
        if (!data) return;
        setCompletion({
          heightDone: data.heightDone === true,
          weightDone: data.weightDone === true,
          chestDone: data.chestDone === true,
        });
      });
  }
};

const handleNextCandidate = () => {
  setRecord(null);
  setShowAdmissionCard(false);

  setFormData({
    application_number: "",
    gender: "",
    height_cm: "",
    chest_cm: "",
    weight_kg: "",
    remarks: "",
  });

  setCompletion({
    heightDone: false,
    weightDone: false,
    chestDone: false,
  });
};

useEffect(() => {
  if (!formData.application_number) return;

  fetch(`${PST_API}/${formData.application_number}`)
    .then(res => res.ok ? res.json() : null)
    .then(data => {
      if (!data) return;

      setFormData(prev => ({
        ...prev,
        height_cm: data.heightCm ?? prev.height_cm,
        weight_kg: data.weightKg ?? prev.weight_kg,
        chest_cm: data.chestCm ?? prev.chest_cm,
        remarks: data.remarks ?? ""   // ✅ ADD THIS
      }));
    });
}, [completion]);



  /* ================= SUBMIT HEIGHT ================= */
  const submitHeight = async () => {
    const res = await fetch(`${PST_API}/height`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        applicationNumber: formData.application_number,
        heightCm: Number(formData.height_cm),
      }),
    });

    if (!res.ok) {
      handleShowToast("Failed to save height", "error");
      return;
    }

    handleShowToast("Height saved successfully", "success");

    // refresh DB truth
    fetch(`${PST_API}/status/${formData.application_number}`)
      .then((r) => (r.ok ? r.json() : null))
      .then((d) => d && setCompletion(d));
  };

  /* ================= SUBMIT WEIGHT ================= */
  const submitWeight = async () => {
    const res = await fetch(`${PST_API}/weight`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        applicationNumber: formData.application_number,
        weightKg: Number(formData.weight_kg),
      }),
    });

    if (!res.ok) {
      handleShowToast("Failed to save weight", "error");
      return;
    }

    handleShowToast("Weight saved successfully", "success");

    fetch(`${PST_API}/status/${formData.application_number}`)
      .then((r) => (r.ok ? r.json() : null))
      .then((d) => d && setCompletion(d));
  };

  /* ================= SUBMIT CHEST ================= */
  const submitChest = async () => {
    const res = await fetch(`${PST_API}/chest`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        applicationNumber: formData.application_number,
        chestCm: Number(formData.chest_cm),
      }),
    });

    if (!res.ok) {
      handleShowToast("Failed to save chest", "error");
      return;
    }

    handleShowToast("Chest saved successfully", "success");

    fetch(`${PST_API}/status/${formData.application_number}`)
      .then((r) => (r.ok ? r.json() : null))
      .then((d) => d && setCompletion(d));
  };

  /* ================= FINAL EVALUATION ================= */
const handleSubmit = async (e) => {
  e.preventDefault();

  const evaluation = evaluatePST(formData);
  const receiptNumber = `REC-${Date.now().toString().slice(-6)}`;

  // 🔑 CALL FINALIZE API (THIS WAS MISSING)
  const res = await fetch(`${PST_API}/finalize`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      applicationNumber: formData.application_number,
      pstStatus: evaluation.status,
      receiptNumber: receiptNumber,
        remarks: formData.remarks, // ✅ ADD
    }),
  });

  if (!res.ok) {
    handleShowToast("Failed to finalize PST", "error");
    return;
  }

  // ✅ NOW update UI
  setRecord({
    ...formData,
    pst_status: evaluation.status,
    measurement_date_time: new Date().toLocaleString(),
    receipt_number: receiptNumber,
  });
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
  placeholder="Enter Application No"
/>
            </div>

            <div className="ledger-row">
              <label>Gender</label>
              <input value={formData.gender} readOnly className="readonly-field" />
            </div>

            <div className="ledger-row">
              <label>
                Height (cm)
                {completion.heightDone && <span className="green-tick">✔</span>}
              </label>
              <input
                type="number"
                name="height_cm"
                value={formData.height_cm}
                onChange={handleChange}
                disabled={completion.heightDone}
              />
              <button
                type="button"
                className="measure-btn"
                onClick={submitHeight}
                disabled={completion.heightDone}
              >
                Submit
              </button>
            </div>

            <div className="ledger-row">
              <label>
                Weight (kg)
                {completion.weightDone && <span className="green-tick">✔</span>}
              </label>
              <input
                type="number"
                name="weight_kg"
                value={formData.weight_kg}
                onChange={handleChange}
                disabled={completion.weightDone}
              />
              <button
                type="button"
                className="measure-btn"
                onClick={submitWeight}
                disabled={completion.weightDone}
              >
                Submit
              </button>
            </div>

            <div className="ledger-row">
              <label>
                Chest (cm)
                {completion.chestDone && <span className="green-tick">✔</span>}
              </label>
              <input
                type="number"
                name="chest_cm"
                value={formData.chest_cm}
                onChange={handleChange}
                disabled={completion.chestDone}
              />
              <button
                type="button"
                className="measure-btn"
                onClick={submitChest}
                disabled={completion.chestDone}
              >
                Submit
              </button>
            </div>

            <div className="ledger-row">
              <label>Remarks</label>
              <input
                name="remarks"
                value={formData.remarks}
                onChange={handleChange}
              />
            </div>
          </div>

          <div className="form-footer">
            <button type="submit" className="btn-process">
              Process Evaluation & Print
            </button>
          </div>
        </form>
            ) : (
        <div className="receipt-area">
          {record.pst_status === "Pass" ? (
            <>
              <AdmissionCard ref={componentRef} candidate={record} />

              <div className="action-buttons no-print">
                <button
                  className="btn-print"
                  onClick={handlePrint}
                >
                  Print Admission Card
                </button>
                 <button
    onClick={handleNextCandidate}
    className="btn-secondary"
  >
    Next Candidate
  </button>
              </div>
            </>
          ) : (
            <>
              <RejectionSlip
                ref={componentRef}
                candidate={record}
                reasons={record.rejection_reasons}
              />

              <div className="action-buttons no-print">
                <button
                  className="btn-print"
                  onClick={handlePrint}
                >
                  Print Rejection Slip
                </button>
                 <button
    onClick={handleNextCandidate}
    className="btn-secondary"
  >
    Next Candidate
  </button>
              </div>
            </>
          )}
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
