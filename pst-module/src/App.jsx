
import PstCandidateList from "./pages/PstCandidateList";
import React, { useState } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";

// Components
import Navbar from "./components/Navbar";
import Login from "./pages/Login"; 

// Pages
import Dashboard from './pages/Dashboard';
import Registration from "./pages/Registration";
import ChestNumberAllocation from "./pages/ChestNumberAllocation";
import BarcodeVerification from "./pages/BarcodeVerification";
import PstModule from "./pages/PstModule";
import VerificationPage from "./pages/VerificationPage"; 
import Phase5Dropdown from "./pages/Phase5Dropdown";
import UploadPage from "./pages/UploadPage"; 
import EventPage from "./pages/EventPage";
// 1. IMPORT YOUR NEW REPORTS PAGE HERE
// import ReportsPage from "./pages/ReportsPage"; 

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  return (
    <div className="app-container">
      {isAuthenticated && <Navbar setIsAuthenticated={setIsAuthenticated} />}

      <main className="content-area">
        <Routes>
          <Route 
            path="/login" 
            element={!isAuthenticated ? <Login setIsAuthenticated={setIsAuthenticated} /> : <Navigate to="/" />} 
          />
<Route path="/pst-candidates" element={<PstCandidateList />} />
<Route path="/pst/:applicationNo" element={<PstModule />} />
          <Route path="/" element={isAuthenticated ? <Dashboard /> : <Navigate to="/login" />} />
          <Route path="/upload" element={isAuthenticated ? <UploadPage /> : <Navigate to="/login" />} /> 
          <Route path="/registration" element={isAuthenticated ? <Registration /> : <Navigate to="/login" />} />
          <Route path="/document-verification" element={isAuthenticated ? <VerificationPage /> : <Navigate to="/login" />} />
          <Route path="/phase5dropdown" element={isAuthenticated ? <Phase5Dropdown /> : <Navigate to="/login" />} />
          <Route path="/chest-allocation" element={isAuthenticated ? <ChestNumberAllocation /> : <Navigate to="/login" />} />
          <Route path="/barcode-verification" element={isAuthenticated ? <BarcodeVerification /> : <Navigate to="/login" />} />
          <Route path="/pst-module" element={isAuthenticated ? <PstModule /> : <Navigate to="/login" />} />
          <Route path="/event" element={isAuthenticated ? <EventPage /> : <Navigate to="/login" />} />
          {/* <Route path="/reports" element={isAuthenticated ? <ReportsPage /> : <Navigate to="/login" />} /> */}

          <Route path="*" element={<Navigate to={isAuthenticated ? "/" : "/login"} />} />
        </Routes>
      </main>

      {isAuthenticated && (
                    <footer className="no-print" style={{ 
                        textAlign: 'center', 
                        padding: '15px', 
                        color: '#1e293b', 
                        fontSize: '0.75rem',
                        background: '#ffffff',
                        fontWeight: '700',
                        borderTop: '2px solid #1e293b',
                        textTransform: 'uppercase',
                        letterSpacing: '2px'
                    }}>
                        RECRUITMENT CONTROL SYSTEM © 2026 | SECURE TERMINAL
                    </footer>
                )}
    </div>
  );
}


export default App;