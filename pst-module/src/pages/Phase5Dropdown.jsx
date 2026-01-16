import React, { useState } from "react";
import "../styles/Phase5Menu.css";

/* ================================
   CONFIG – API BASE URL (VITE SAFE)
   ================================ */
const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL?.replace(/\/$/, "") ||
  "http://localhost:5000";

/* ---------- Reusable Dropdown ---------- */
function Dropdown({ title, items, onItemClick }) {
  const [open, setOpen] = useState(false);

  return (
    <div className="dropdown">
      <button
        type="button"
        className="dropdown-btn"
        onClick={() => setOpen((prev) => !prev)}
      >
        {title}
        <span className="arrow">{open ? "▲" : "▼"}</span>
      </button>

      {open && (
        <ul className="dropdown-menu">
          {items.map((item, index) => (
            <li
              key={index}
              className="dropdown-item"
              onClick={() => {
                setOpen(false);
                onItemClick(item);
              }}
            >
              {item}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

/* ---------- Phase 5 Component ---------- */
export default function Phase5Dropdown() {
  /* ================================
     PDF DOWNLOAD HANDLER
     ================================ */
  const downloadReport = async (type) => {
    try {
      let url = "";
      let fileName = "";

      switch (type) {
        case "passed":
          url = `${API_BASE_URL}/api/reports/passed-candidates-pdf`;
          fileName = "All_Passed_Candidates.pdf";
          break;

        case "failed":
          url = `${API_BASE_URL}/api/reports/failed-candidates-pdf`;
          fileName = "All_Failed_Candidates.pdf";
          break;

        case "total-registration":
          url = `${API_BASE_URL}/api/reports/total-registration-pdf`;
          fileName = "Total_Registration.pdf";
          break;

        case "selected":
          url = `${API_BASE_URL}/api/reports/selected-candidates-pdf`;
          fileName = "Selected_Candidates_List.pdf";
          break;

        case "rejected":
          url = `${API_BASE_URL}/api/reports/rejected-candidates-pdf`;
          fileName = "Rejected_Candidates_List.pdf";
          break;

        case "verified":
          url = `${API_BASE_URL}/api/reports/verified-candidates-pdf`;
          fileName = "Verified_Candidates_List.pdf";
          break;

        case "unverified":
          url = `${API_BASE_URL}/api/reports/unverified-candidates-pdf`;
          fileName = "Unverified_Candidates_List.pdf";
          break;

        case "pet-event-wise":
          url = `${API_BASE_URL}/api/reports/pet-event-wise-report-pdf`;
          fileName = "PET_Event_Wise_Report.pdf";
          break;

        case "pet-final-constable":
          url = `${API_BASE_URL}/api/reports/pet-final-constable-pdf`;
          fileName = "PET_Final_Constable.pdf";
          break;

        case "pet-final-driver":
          url = `${API_BASE_URL}/api/reports/pet-final-driver-pdf`;
          fileName = "PET_Final_Driver.pdf";
          break;

        case "audit-report":
          url = `${API_BASE_URL}/api/reports/audit-report-pdf`;
          fileName = "Audit_Report.pdf";
          break;

        case "final-selection":
          url = `${API_BASE_URL}/api/reports/final-selection-pdf`;
          fileName = "Final_Selection.pdf";
          break;

        

        default:
          alert("Invalid report type");
          return;
      }

      const response = await fetch(url);

      if (!response.ok) {
        const msg = await response.text();
        alert(msg || "No data available");
        return;
      }

      const blob = await response.blob();
      const objectUrl = window.URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = objectUrl;
      link.download = fileName;
      document.body.appendChild(link);
      link.click();
      link.remove();

      window.URL.revokeObjectURL(objectUrl);
    } catch (err) {
      console.error(err);
      alert("Failed to download PDF");
    }
  };

  /* ================================
     CLICK HANDLER
     ================================ */
  const handleClick = (item) => {
    const map = {
      "Total Registration": "total-registration",
      "All Passed Candidates": "passed",
      "All Failed Candidates": "failed",
      "Selected Candidates List": "selected",
      "Rejected Candidates List": "rejected",
      "Verified Candidates List": "verified",
      "Unverified Candidates List": "unverified",
      "PET Event Wise Report": "pet-event-wise",
      "Constable": "pet-final-constable",
      "Driver": "pet-final-driver",
      "Audit Report": "audit-report",
      "Overall Rank / Final Selection":"final-selection"
    };

    const key = map[item];
    if (!key) {
      alert(`Unknown option: ${item}`);
      return;
    }

    downloadReport(key);
  };

  /* ================================
     DROPDOWN ITEMS
     ================================ */
  const recordFetchItems = [
    "Total Registration",
    "Selected Candidates List",
    "Rejected Candidates List",
  ];

  const biometricItems = [
    "Verified Candidates List",
    "Unverified Candidates List",
  ];

  const petReportItems = ["PET Event Wise Report"];

  const petFinalStatusItems = [
    "All Passed Candidates",
    "All Failed Candidates",
    "Constable",
    "Driver",
  ];

  const administrativeItems = ["Audit Report"];

  const finalMeritItems = ["Overall Rank / Final Selection"];

  return (
    <div className="container">
      <h2 className="title">FINAL REPORT</h2>

      <Dropdown title="Record Fetch" items={recordFetchItems} onItemClick={handleClick} />
      <Dropdown title="Biometric Report" items={biometricItems} onItemClick={handleClick} />
      <Dropdown title="PET Report" items={petReportItems} onItemClick={handleClick} />
      <Dropdown title="PET Final Status" items={petFinalStatusItems} onItemClick={handleClick} />
      <Dropdown title="Administrative Report" items={administrativeItems} onItemClick={handleClick} />
      <Dropdown title="Final Merit" items={finalMeritItems} onItemClick={handleClick} />
    </div>
  );
}
