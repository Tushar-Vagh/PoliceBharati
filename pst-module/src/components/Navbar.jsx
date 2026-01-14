import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import '../styles/navbar.css';

const Navbar = ({ setIsAuthenticated }) => { // 1. Receive setter prop
  const location = useLocation();
  const navigate = useNavigate();

  const showBackButton = location.pathname !== "/";
  
  const handleLogout = () => {
    const confirmLogout = window.confirm("Are you sure you want to logout?");
    if (confirmLogout) {
      setIsAuthenticated(false); // 2. Clear auth state
      navigate("/login"); // 3. Redirect to login
    }
  };

  return (
    <nav className="navbar no-print">
      <div className="nav-container">
        
        {/* LEFT: Logo Section */}
        <div className="nav-section nav-left">
          <img src="src/assets/logo.JPG" alt="Logo" className="logo-img" />
        </div>

        {/* CENTER: Main Title Section */}
        <div className="nav-section nav-center">
          <h1 className="nav-title">RECRUITMENT CONTROL PANEL</h1>
        </div>

        {/* RIGHT: Action Section */}
        <div className="nav-section nav-right">
          {showBackButton && (
            <button className="nav-back-btn" onClick={() => navigate('/')}>
              BACK TO DASHBOARD <span className="arrow">→</span>
            </button>
          )}
          
          {/* 4. Added Logout Button */}
          <button className="nav-logout-btn" onClick={handleLogout}>
            LOGOUT <span className="icon">⏻</span>
          </button>
        </div>

      </div>
    </nav>
  );
};

export default Navbar;