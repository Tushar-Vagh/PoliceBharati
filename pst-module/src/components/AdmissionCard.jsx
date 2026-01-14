import React, { forwardRef, useEffect, useState } from 'react';

const AdmissionCard = forwardRef(({ candidate }, ref) => {
    const [master, setMaster] = useState(null);

    /* ================= FETCH MASTER DATA ================= */
    useEffect(() => {
        if (!candidate?.application_number) return;

        const fetchMaster = async () => {
            try {
                const res = await fetch(
                    `/api/Master/by-application/${candidate.application_number}`
                );
                if (!res.ok) return;
                const data = await res.json();
                setMaster(data);
            } catch (err) {
                console.error("Failed to load master data", err);
            }
        };

        fetchMaster();
    }, [candidate]);

    if (!candidate || !master) return null;

    /* ================= UI STYLES (UNCHANGED) ================= */
    const styles = {
        container: {
            fontFamily: 'Arial, sans-serif',
            maxWidth: '210mm',
            margin: '0 auto',
            padding: '20px',
            backgroundColor: 'white',
            color: '#000',
        },
        header: {
            display: 'flex',
            justifyContent: 'space-between',
            marginBottom: '20px',
            fontSize: '0.8rem',
            color: '#333'
        },
        titleBox: {
            borderBottom: '1px solid #ccc',
            padding: '15px',
            textAlign: 'center',
            backgroundColor: '#fff',
        },
        titleText: {
            fontSize: '1.5rem',
            fontWeight: 'bold',
            color: '#333',
            margin: 0
        },
        section: {
            marginBottom: '25px',
        },
        sectionHeader: {
            border: '1px solid #eee',
            borderLeft: '5px solid #333',
            backgroundColor: '#fff',
            padding: '10px 15px',
            fontWeight: 'bold',
            fontSize: '1.1rem',
            marginBottom: '15px',
            boxShadow: '0 2px 4px rgba(0,0,0,0.05)'
        },
        infoRow: {
            display: 'grid',
            gridTemplateColumns: '200px 1fr',
            marginBottom: '12px',
            alignItems: 'baseline'
        },
        label: {
            color: '#555',
            fontSize: '0.9rem'
        },
        value: {
            fontWeight: '500',
            fontSize: '1rem'
        },
        gridTwo: {
            display: 'grid',
            gridTemplateColumns: '1fr 1fr',
            gap: '20px',
            marginBottom: '15px'
        }
    };

    return (
        <div ref={ref} style={styles.container}>

            {/* Header */}
            <div style={styles.header}>
                <span>{new Date().toLocaleString()}</span>
                <span>Police Recruitment</span>
            </div>

            <div style={{ border: '2px solid #ccc', borderRadius: '4px' }}>
                <div style={styles.titleBox}>
                    <h1 style={styles.titleText}>Admission Card</h1>
                </div>

                <div style={{ padding: '20px' }}>

                    {/* Basic Info */}
                    <div style={styles.gridTwo}>
                        <div>
                            <span style={styles.label}>Recruitment Year:</span>
                            <div style={styles.value}>2025-2026</div>
                        </div>
                        <div>
                            <span style={styles.label}>Application No:</span>
                            <div style={styles.value}>{master.applicationNo}</div>
                        </div>
                    </div>

                    {/* PERSONAL INFORMATION */}
                    <div style={styles.section}>
                        <div style={styles.sectionHeader}>Personal Information</div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Candidate's Full Name (Devnagari):</span>
                            <span style={styles.value}>
                                {master.firstName_Marathi} {master.fatherName_Marathi} {master.surname_Marathi}
                            </span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Candidate's Full Name (English):</span>
                            <span style={styles.value}>
                                {master.firstName_English} {master.fatherName_English} {master.surname_English}
                            </span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Mother Name:</span>
                            <span style={styles.value}>{master.motherName_English}</span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Gender:</span>
                            <span style={styles.value}>{master.gender}</span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Date of Birth:</span>
                            <span style={styles.value}>{master.dob}</span>
                        </div>
                    </div>

                    {/* CONTACT ADDRESS */}
                    <div style={styles.section}>
                        <div style={styles.sectionHeader}>Address for Contact</div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Address:</span>
                            <span style={styles.value}>
                                {master.address1}, {master.address2}, {master.address3}
                            </span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Pin Code:</span>
                            <span style={styles.value}>{master.pinCode}</span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Mobile:</span>
                            <span style={styles.value}>{master.mobileNo}</span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Email:</span>
                            <span style={styles.value}>{master.emailID}</span>
                        </div>
                    </div>

                    {/* PERMANENT ADDRESS */}
                    <div style={styles.section}>
                        <div style={styles.sectionHeader}>Permanent Address</div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Address:</span>
                            <span style={styles.value}>
                                {master.permanantAddress1}, {master.permanantAddress2}
                            </span>
                        </div>

                        <div style={styles.infoRow}>
                            <span style={styles.label}>Pin Code:</span>
                            <span style={styles.value}>{master.permanantPinCode}</span>
                        </div>
                    </div>

                    {/* OTHER INFORMATION */}
                    <div style={styles.section}>
                        <div style={styles.sectionHeader}>Other Information</div>

                        <div style={styles.infoRow}><span style={styles.label}>Religion:</span><span style={styles.value}>{master.religion}</span></div>
                        <div style={styles.infoRow}><span style={styles.label}>Caste:</span><span style={styles.value}>{master.caste}</span></div>
                        <div style={styles.infoRow}><span style={styles.label}>Sub Caste:</span><span style={styles.value}>{master.subCaste}</span></div>
                        <div style={styles.infoRow}><span style={styles.label}>Part Time:</span><span style={styles.value}>{master.parttime ? 'Yes' : 'No'}</span></div>
                        <div style={styles.infoRow}><span style={styles.label}>Ex-Serviceman:</span><span style={styles.value}>{master.exSoldier ? 'Yes' : 'No'}</span></div>
                    </div>

                </div>
            </div>
        </div>
    );
});

export default AdmissionCard;
