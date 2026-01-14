// import React from 'react';
// import { useNavigate } from 'react-router-dom';
// // Asset imports
// import uploadGif from '../assets/upload.gif'; 
// import registrationGif from '../assets/registration.gif';
// import verificationGif from '../assets/verification.gif';
// import allocationGif from '../assets/chest.png';
// import barcodeGif from '../assets/barcode.gif';
// import pstGif from '../assets/pst.gif';
// import eventGif from '../assets/running.gif';
// // 1. ADD NEW ICON IMPORT HERE (Ensure you have this file in your assets)
// import reportGif from '../assets/report.gif'; 

// import '../styles/dashboard.css';

// const Dashboard = () => {
//   const navigate = useNavigate();

//   const modules = [
//     { path: '/upload', title: 'Upload', src: uploadGif },
//     { path: '/registration', title: 'Candidate Registration', src: registrationGif },
//     { path: '/document-verification', title: 'Document Verification', src: verificationGif },
//     { path: '/chest-allocation', title: 'Chest No. Allocation', src: allocationGif },
//     { path: '/barcode-verification', title: 'Barcode Verification', src: barcodeGif },
//     { path: '/pst-module', title: 'PST Module', src: pstGif },
//     { path: '/event', title: 'Events', src: eventGif },
//     // 2. NEW REPORT CARD ADDED HERE
//     { path: '/phase5dropdown', title: 'Reports & Analytics', src: reportGif }, 
//   ];

//   return (
//     <div className="dashboard-container">
//       <div className="module-grid">
//         {modules.map((module) => (
//           <button 
//             key={module.path} 
//             className="module-btn"
//             onClick={() => navigate(module.path)}
//           >
//             <div className="icon-circle">
//               <img src={module.src} alt={module.title} className="plain-img-icon" />
//             </div>
//             <div className="module-title">{module.title}</div>
//           </button>
//         ))}
//       </div>
//     </div>
//   );
// };

// export default Dashboard;


import React from 'react';
import { useNavigate } from 'react-router-dom';

// ✅ Asset imports
import uploadGif from '../assets/upload.gif';
import registrationGif from '../assets/registration.gif';
import verificationGif from '../assets/verification.gif';
import allocationGif from '../assets/chest.png';
import barcodeGif from '../assets/barcode.gif';
import pstGif from '../assets/pst.gif';
import eventGif from '../assets/running.gif';
import reportGif from '../assets/report.gif'; // Ensure this file exists

import '../styles/dashboard.css';

const Dashboard = () => {
    const navigate = useNavigate();
const user = JSON.parse(localStorage.getItem("user"));

    // ✅ Step 1: handle missing user
    if (!user) {
        return (
            <div className="dashboard-container">
                <p className="no-access-msg">User not found. Please login again.</p>
            </div>
        );
    }

    // ✅ All available modules
    const modules = [
        { path: '/upload', title: 'Upload', src: uploadGif },
        { path: '/registration', title: 'Candidate Registration', src: registrationGif },
        { path: '/document-verification', title: 'Document Verification', src: verificationGif },
        { path: '/chest-allocation', title: 'Chest No. Allocation', src: allocationGif },
        { path: '/barcode-verification', title: 'Barcode Verification', src: barcodeGif },
    { path: '/pst-candidates', title: 'PST Module', src: pstGif },
    

        { path: '/event', title: 'Events', src: eventGif },
        { path: '/phase5dropdown', title: 'Reports & Analytics', src: reportGif },
    ];

    // ✅ Role-based access map
    const accessMap = {
        admin: modules.map(m => m.path),
        phaseone: ['/upload', '/registration', '/document-verification','/barcode-verification'],
phase2: ['/pst-candidates', '/document-verification'],

        phase3: ['/chest-allocation'],
        phase4: ['/event'],
        phase5: ['/phase5dropdown'],
    };

    // ✅ Normalize username and filter modules
    const normalizedUsername = (user?.Username || user?.username || "").trim().toLowerCase();
    const allowedPaths = accessMap[normalizedUsername] || [];
    const visibleModules = modules.filter(module => allowedPaths.includes(module.path));

    return (
        <div className="dashboard-container">
            <h2 className="dashboard-title">Welcome , {user?.Username || user?.username}</h2>
            <div className="module-grid">
                {visibleModules.length === 0 ? (
                    <p className="no-access-msg">No modules available for your role.</p>
                ) : (
                    visibleModules.map((module) => (
                        <button
                            key={module.path}
                            className="module-btn"
                            onClick={() => navigate(module.path)}
                        >
                            <div className="icon-circle">
                                <img src={module.src} alt={module.title} className="plain-img-icon" />
                            </div>
                            <div className="module-title">{module.title}</div>
                        </button>
                    ))
                )}
            </div>
        </div>
    );
};

export default Dashboard;

