import React, { useState } from 'react';
//import '../styles/reports.css';

const ReportsPage = () => {
  // Mock data - In a real app, you would fetch this from your database/localStorage
  const [reportData] = useState([
    { id: 1, name: "Rahul Sharma", appNo: "APP-2026-001", status: "PASS", height: "172", chest: "84" },
    { id: 2, name: "Amit Patil", appNo: "APP-2026-002", status: "FAIL", height: "162", chest: "78" },
    { id: 3, name: "Sandeep Vichare", appNo: "APP-2026-003", status: "PASS", height: "168", chest: "81" },
  ]);

  const stats = {
    total: reportData.length,
    passed: reportData.filter(d => d.status === "PASS").length,
    failed: reportData.filter(d => d.status === "FAIL").length,
  };

  return (
    <div className="reports-container">
      <div className="reports-header no-print">
        <h2>Recruitment Analytics & Reports</h2>
        <button className="btn-print-report" onClick={() => window.print()}>Export to PDF</button>
      </div>

      {/* Stats Cards */}
      <div className="stats-grid no-print">
        <div className="stat-card">
          <h3>Total Candidates</h3>
          <p className="stat-val">{stats.total}</p>
        </div>
        <div className="stat-card pass">
          <h3>Qualified (Pass)</h3>
          <p className="stat-val">{stats.passed}</p>
        </div>
        <div className="stat-card fail">
          <h3>Disqualified (Fail)</h3>
          <p className="stat-val">{stats.failed}</p>
        </div>
      </div>

      {/* Main Table */}
      <div className="report-table-wrapper">
        <h3 className="table-title">Daily PST Evaluation Log - 2026</h3>
        <table className="report-table">
          <thead>
            <tr>
              <th>App No.</th>
              <th>Candidate Name</th>
              <th>Height (cm)</th>
              <th>Chest (cm)</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {reportData.map((item) => (
              <tr key={item.id}>
                <td>{item.appNo}</td>
                <td>{item.name}</td>
                <td>{item.height}</td>
                <td>{item.chest}</td>
                <td>
                  <span className={`status-pill ${item.status.toLowerCase()}`}>
                    {item.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ReportsPage;