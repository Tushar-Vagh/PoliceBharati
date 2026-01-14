import React, { useEffect } from 'react';
import '../styles/pst.css';

const Toast = ({ message, type = 'info', duration = 4000, onClose }) => {
    useEffect(() => {
        if (!message) return;
        const t = setTimeout(() => onClose && onClose(), duration);
        return () => clearTimeout(t);
    }, [message, duration, onClose]);

    if (!message) return null;

    return (
        <div className={`toast ${type}`} role="status" aria-live="polite">
            <div className="toast-content">
                <div className="toast-message">{message}</div>
                <button className="toast-close" onClick={onClose} aria-label="Close">OK</button>
            </div>
        </div>
    );
};

export default Toast;