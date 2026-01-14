import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { calculateDuration, getRunningMarks } from '../utils/eventCalculations';
import '../styles/event.css';

// Asset imports
import runningGif from '../assets/running.gif'; 
import shotputGif from '../assets/shotput.gif'; 

const EventPage = () => {
    const navigate = useNavigate();
    const [view, setView] = useState('sections'); 
    const [selectedEvent, setSelectedEvent] = useState('');
    
    const [formData, setFormData] = useState({
        appNumber: '',
        chestNumber: '',
        gender: '', 
        startTime: '',
        endTime: '',
        duration: '',
        marks: '',
        remarks: ''
    });

    // Mock Gender/Chest Fetch logic
    useEffect(() => {
        if (formData.appNumber.length >= 3) {
            const mockData = {
                '101': { chest: 'C-001', gender: 'male' },
                '102': { chest: 'C-002', gender: 'female' },
                '103': { chest: 'C-003', gender: 'male' }
            };
            const student = mockData[formData.appNumber];
            if (student) {
                setFormData(prev => ({ 
                    ...prev, 
                    chestNumber: student.chest, 
                    gender: student.gender 
                }));
            }
        } else {
            setFormData(prev => ({ ...prev, gender: '', chestNumber: '' }));
        }
    }, [formData.appNumber]);

    // Duration & Marks Calculation logic
    useEffect(() => {
        if (formData.startTime && formData.endTime) {
            const { durationStr, totalSeconds } = calculateDuration(formData.startTime, formData.endTime);
            const { marks, remarks } = getRunningMarks(selectedEvent, totalSeconds);
            setFormData(prev => ({ ...prev, duration: durationStr, marks, remarks }));
        }
    }, [formData.startTime, formData.endTime, selectedEvent]);

    const resetToCategories = () => {
        setFormData({ appNumber: '', chestNumber: '', gender: '', startTime: '', endTime: '', duration: '', marks: '', remarks: '' });
        setView('sections');
        setSelectedEvent('');
    };

    const handleRaceSelection = (distance) => {
        if (!formData.appNumber || formData.appNumber.trim() === "") {
            alert("⚠️ COMPULSORY FIELD\nPlease enter the Application Number first.");
            return;
        }
        if (distance === '1600m' && formData.gender === 'female') {
            alert("⚠️ NOT APPLICABLE\n1600m event is not available for female candidates.");
            return;
        }
        setSelectedEvent(distance);
        setView('form');
    };

    return (
        <div className="event-container">
            <main className="event-main-content">
                
                {/* STEP 1: CATEGORY SELECTION */}
                {view === 'sections' && (
                    <div className="selection-wrapper">
                        <div className="category-grid">
                            <button className="category-card" onClick={() => setView('sub-events')}>
                                <div className="icon-circle"><img src={runningGif} alt="Running" /></div>
                                <span className="card-label">RUNNING</span>
                            </button>
                            <button className="category-card" onClick={() => { setSelectedEvent('Shotput'); setView('form'); }}>
                                <div className="icon-circle"><img src={shotputGif} alt="Shotput" /></div>
                                <span className="card-label">SHOTPUT</span>
                            </button>
                        </div>
                    </div>
                )}

                {/* STEP 2: DISTANCE SELECTION */}
                {view === 'sub-events' && (
                    <div className="selection-wrapper">
                        <div className="combined-card">
                            <div className="app-input-section">
                                <label className="input-label-top">ENTER APPLICATION NUMBER FIRST</label>
                                <input 
                                    type="text" 
                                    placeholder="Enter Application Number" 
                                    className="app-input-field small-input"
                                    value={formData.appNumber} 
                                    onChange={(e) => setFormData({...formData, appNumber: e.target.value})}
                                />
                            </div>

                            <hr className="divider-line" />

                            <div className="distance-section">
                                <h3 className="select-distance-label">SELECT DISTANCE</h3>
                                <div className="dist-btn-row">
                                    <button className="wide-pill-btn" onClick={() => handleRaceSelection('100m')}>100m</button>
                                    <button className="wide-pill-btn" onClick={() => handleRaceSelection('800m')}>800m</button>
                                    <button 
                                        className={`wide-pill-btn ${formData.gender === 'female' ? 'restricted' : ''}`} 
                                        onClick={() => handleRaceSelection('1600m')}
                                    >
                                        1600m
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                )}

                {/* STEP 3: DATA ENTRY FORM (Updated with step="1" for HH:mm:ss) */}
                {view === 'form' && (
                    <div className="form-container-card">
                        <div className="form-details-header">
                            <h3>Entry Form: {selectedEvent}</h3>
                            <div className="student-info-strip">
                                <span><strong>App:</strong> {formData.appNumber}</span>
                                <span><strong>Chest:</strong> {formData.chestNumber}</span>
                                <span><strong>Gender:</strong> {formData.gender.toUpperCase()}</span>
                            </div>
                        </div>
                        
                        <div className="event-form-grid">
                            <div className="input-group">
                                <label>Start Time (HH:mm:ss)</label>
                                <input 
                                    type="time" 
                                    step="1" 
                                    value={formData.startTime} 
                                    onChange={(e) => setFormData({...formData, startTime: e.target.value})} 
                                />
                            </div>
                            <div className="input-group">
                                <label>End Time (HH:mm:ss)</label>
                                <input 
                                    type="time" 
                                    step="1" 
                                    value={formData.endTime} 
                                    onChange={(e) => setFormData({...formData, endTime: e.target.value})} 
                                />
                            </div>
                            <div className="input-group highlight-box">
                                <label>Duration</label>
                                <input type="text" value={formData.duration} readOnly />
                            </div>
                            <div className="input-group highlight-box">
                                <label>Marks</label>
                                <input type="text" value={formData.marks} readOnly />
                            </div>
                            <div className="input-group full-width">
                                <label>Remarks</label>
                                <textarea value={formData.remarks} readOnly />
                            </div>
                        </div>

                        <div className="form-actions-footer">
                            <button className="save-btn">SAVE RESULTS</button>
                            <button className="cancel-btn" onClick={resetToCategories}>CANCEL</button>
                        </div>
                    </div>
                )}
            </main>
        </div>
    );
};

export default EventPage;