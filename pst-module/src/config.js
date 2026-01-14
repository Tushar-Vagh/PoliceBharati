// Basic API Configuration
// Ensure your .NET backend is running on this port
export const API_BASE_URL = "http://localhost:5000";

export const endpoints = {
    candidate: (id) => `${API_BASE_URL}/api/candidates/${id}`,
    verify: `${API_BASE_URL}/api/candidates/verify`
};