import React, { useState, useRef } from "react";
import "../styles/upload.css";

const UploadPage = () => {
  const fileInputRef = useRef(null);
  const [file, setFile] = useState(null);
  const [uploading, setUploading] = useState(false);
  const [result, setResult] = useState(null);

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
    setResult(null);
  };

  const handleUpload = async () => {
  if (!file) {
    alert("Please select a file first.");
    return;
  }

  setUploading(true);
  setResult(null);

  try {
    const formData = new FormData();
    formData.append("File", file);

    const response = await fetch(
      "http://localhost:5000/api/master/bulk-upload",
      {
        method: "POST",
        body: formData,
      }
    );

    if (!response.ok) {
      const text = await response.text();
      throw new Error(text || "Upload failed");
    }

    const data = await response.json();
    setResult(data);
  } catch (error) {
    console.error("Upload error:", error);
    alert("❌ Upload failed. Check backend logs.");
  } finally {
    // ✅ THIS MUST ALWAYS RUN
    setUploading(false);

    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }

    setFile(null);
  }
};


  return (
    <div className="admin-layout">
      <main className="upload-container">
        <div className="upload-card">
          <div className="upload-icon-section">
            <div className="icon-circle">
              <span className="upload-arrow">↑</span>
            </div>
            <h2 className="upload-main-title">Candidate Data Import</h2>
            <p className="upload-subtitle">
              Supported formats: .xlsx (recommended)
            </p>
          </div>

          <div className="drop-zone">
            <input
              ref={fileInputRef}
              type="file"
              id="fileInput"
              className="hidden-input"
              onChange={handleFileChange}
              accept=".xlsx"
            />

            <label htmlFor="fileInput" className="file-label">
              {file
                ? `Selected file: ${file.name}`
                : "Click to browse or drag file here"}
            </label>
          </div>

          <button
            className={`action-btn ${uploading ? "loading" : ""}`}
            onClick={handleUpload}
            disabled={uploading}
          >
            {uploading ? "Uploading..." : "Start Upload"}
          </button>

          {file && !uploading && (
            <button
              className="clear-btn"
              onClick={() => {
                setFile(null);
                if (fileInputRef.current) {
                  fileInputRef.current.value = "";
                }
              }}
            >
              Remove File
            </button>
          )}

          {/* ✅ RESULT SUMMARY */}
          {result && (
            <div className="upload-result">
              <h4>Upload Summary</h4>
              <p>📄 Total Rows: {result.totalRows}</p>
              <p>✅ Inserted: {result.inserted}</p>
              <p>❌ Failed: {result.failed}</p>
            </div>
          )}
        </div>
      </main>
    </div>
  );
};

export default UploadPage;
