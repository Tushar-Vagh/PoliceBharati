import React, { useState } from "react";
import "../styles/Phase5Menu.css";

/* ---------- Reusable Dropdown ---------- */
function Dropdown({ title, items, onItemClick }) {
  const [open, setOpen] = useState(false);

  return (
    <div className="dropdown">
      <button
        type="button"
        className="dropdown-btn"
        onClick={() => setOpen(!open)}
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

  /* ===== EXISTING LOGIC – EXTENDED (NOT BROKEN) ===== */
  const downloadExcel = async (type) => {
    try {
      let url = "";
      let fileName = "";

      if (type === "passed") {
        url = "http://localhost:5173/api/reports/passed-candidates";
        fileName = "All_Passed_Candidates.xlsx";
      } 
      else if (type === "failed") {
        url = "http://localhost:5173/api/reports/failed-candidates";
        fileName = "All_Failed_Candidates.xlsx";
      } 
      else if (type === "total-registration") {
        url = "http://localhost:5173/api/reports/total-registration";
        fileName = "Total_Registration.xlsx";
      } 

      else if (type === "selected") {
  url = "http://localhost:5173/api/reports/selected-candidates";
  fileName = "Selected_Candidates_List.xlsx";
}

else if (type === "rejected") {
  url = "http://localhost:5173/api/reports/rejected-candidates";
}

else if (type === "verified") {
  url = "http://localhost:5173/api/reports/verified-candidates";
}

else if (type === "unverified") {
  url = "http://localhost:5173/api/reports/unverified-candidates";
}

else if (type === "pet-event-wise") {
  url = "http://localhost:5173/api/reports/pet-event-wise-report";
}

else if (type === "PET_Final_Constable") {
  url = "http://localhost:5173/api/reports/pet-final-constable";
}

else if (type === "petfinal--driver") {
  url = "http://localhost:5173/api/reports/pet-final-driver";

}


      else {
        alert(`Clicked: ${type}`);
        return;
      }
      

      const response = await fetch(url);

// ===== EMPTY EXCEL VALIDATION =====
if (response.status === 404) {
  const message = await response.text();
  alert(message);
  return;
}

if (!response.ok) throw new Error("Download failed");

      const blob = await response.blob();
      const downloadUrl = window.URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = downloadUrl;
      link.download = fileName;

      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (error) {
      alert("Error downloading Excel");
      console.error(error);
    }
  };

  /* ===== CLICK HANDLER ===== */
  const handleClick = (item) => {
    switch (item) {
      case "Total Registration":
        downloadExcel("total-registration");
        break;

      case "All Passed Candidates":
        downloadExcel("passed");
        break;

      case "All Failed Candidates":
        downloadExcel("failed");
        break;

      case "Selected Candidates List":
        downloadExcel("selected");
        break;

      case "Rejected Candidates List":
  downloadExcel("rejected");
  break;

  case "Verified Candidates List":
  downloadExcel("verified");
  break;

  case "Unverified Candidates List":
  downloadExcel("unverified");
  break;

      case "PET Event Wise Report":
  downloadExcel("pet-event-wise");
  break;

  case "Constable":
      downloadExcel("PET_Final_Constable");
      break;

      case "Driver":
  downloadExcel("petfinal--driver");
  break;


      default:
        alert(`Clicked: ${item}`);
    }
  };

  /* ===== PHASE-5 SUB ITEMS ===== */
  const recordFetchItems = [
    "Total Registration",
    "Selected Candidates List",
    "Rejected Candidates List"
  ];

  const biometricItems = [
    "Verified Candidates List",
    "Unverified Candidates List"
  ];

  const petReportItems = [
    "PET Event Wise Report"
  ];

  const petFinalStatusItems = [
    "All Passed Candidates",
    "All Failed Candidates",
    "Constable",
    "Driver"
  ];

  const administrativeItems = [
    // "Administrative Report",
    "Audit Report"
  ];

  const finalMeritItems = [
    "Final Merit Report",
    "Overall Rank / Final Selection"
  ];

  return (
    <div className="container">
      <h2 className="title">FINAL REPORT</h2>

      <Dropdown
        title="Record Fetch"
        items={recordFetchItems}
        onItemClick={handleClick}
      />

      <Dropdown
        title="Biometric Report"
        items={biometricItems}
        onItemClick={handleClick}
      />

      <Dropdown
        title="PET Report"
        items={petReportItems}
        onItemClick={handleClick}
      />

      <Dropdown
        title="PET Final Status"
        items={petFinalStatusItems}
        onItemClick={handleClick}
      />

      <Dropdown
        title="Administrative Report"
        items={administrativeItems}
        onItemClick={handleClick}
      />

      <Dropdown
        title="Final Merit"
        items={finalMeritItems}
        onItemClick={handleClick}
      />
    </div>
  );
}