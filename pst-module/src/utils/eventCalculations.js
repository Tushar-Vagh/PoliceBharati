/**
 * Utility functions for Event Module Calculations
 */

export const calculateDuration = (startTime, endTime) => {
    if (!startTime || !endTime) return { durationStr: '', totalSeconds: 0 };

    const start = new Date(`2026-01-01T${startTime}`);
    const end = new Date(`2026-01-01T${endTime}`);
    const diffMs = end - start;

    if (diffMs <= 0) return { durationStr: 'Invalid Time', totalSeconds: 0 };

    const totalSeconds = Math.floor(diffMs / 1000);
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    // Returns hours only if they exist, otherwise just mins and secs
    const durationStr = hours > 0 
        ? `${hours}h ${minutes}m ${seconds}s` 
        : `${minutes}m ${seconds}s`;

    return {
        durationStr: durationStr,
        totalSeconds: totalSeconds
    };
};

export const getRunningMarks = (eventId, totalSeconds) => {
    if (totalSeconds <= 0) return { marks: 0, remarks: 'N/A' };

    let marks = 0;

    // Logic for 1600m
    if (eventId === '1600m') {
        if (totalSeconds <= 360) marks = 10;      // Under 6 mins
        else if (totalSeconds <= 480) marks = 5;  // Under 8 mins
        else marks = 0;
    } 
    // Logic for 100m
    else if (eventId === '100m') {
        if (totalSeconds <= 12) marks = 10;
        else if (totalSeconds <= 15) marks = 5;
        else marks = 0;
    }
    // Logic for Shotput
    else if (eventId === 'Shotput') {
        if (totalSeconds <= 300) marks = 10; // Pass if under 5 minutes
        else marks = 0;
    }

    return {
        marks: marks,
        remarks: marks > 0 ? 'PASSED' : 'FAILED'
    };
};