import React, { useState, useRef } from "react";
import "../styles/upload.css";

const UploadPage = () => {
  const fileInputRef = useRef(null);
  const errorRef = useRef(null);

  const [file, setFile] = useState(null);
  const [uploading, setUploading] = useState(false);
  const [result, setResult] = useState(null);
  const [error, setError] = useState("");

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
    setResult(null);
    setError("");
  };

  const handleUpload = async () => {
    if (!file) {
      setError("Please select a file first.");
      return;
    }

    setUploading(true);
    setResult(null);
    setError("");

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

      // ❌ BACKEND VALIDATION / SERVER ERROR
      if (!response.ok) {
        let errorMessage = `Upload failed (HTTP ${response.status})`;

        try {
          const contentType = response.headers.get("content-type");

          if (contentType && contentType.includes("application/json")) {
            const json = await response.json();
            if (json?.message) errorMessage = json.message;
          } else {
            const text = await response.text();
            if (text) errorMessage = text;
          }
        } catch {
          // ignore parsing errors
        }

        setError(errorMessage);

        setTimeout(() => {
          errorRef.current?.scrollIntoView({
            behavior: "smooth",
            block: "center",
          });
        }, 100);

        return;
      }

      const data = await response.json();
      setResult(data);
    } catch (err) {
      console.error("Upload error:", err);

      // ✅ SHOW ACTUAL REASON
      let message = "Unable to upload file";

      if (err?.message) {
        if (err.message.includes("Failed to fetch")) {
          message = "Server not reachable. Please try again later.";
        } else {
          message = err.message;
        }
      }

      setError(message);

      setTimeout(() => {
        errorRef.current?.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
      }, 100);
    } finally {
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
                setError("");
                if (fileInputRef.current) {
                  fileInputRef.current.value = "";
                }
              }}
            >
              Remove File
            </button>
          )}

          {/* ❌ INLINE ERROR MESSAGE */}
          {error && (
            <div
              ref={errorRef}
              style={{
                marginTop: "15px",
                padding: "10px",
                border: "1px solid #f44336",
                color: "#f44336",
                borderRadius: "4px",
                fontSize: "14px",
              }}
            >
              ❌ {error}
            </div>
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
