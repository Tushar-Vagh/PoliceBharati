import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/login.css";

const Login = ({ setIsAuthenticated }) => {
    const [loginId, setLoginId] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
    e.preventDefault();

    try {
        const response = await fetch("http://localhost:5000/api/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username: loginId, password }),
        });

        if (response.ok) {
            const user = await response.json();
            console.log("✅ Logged in user object:", user);

            // ✅ STORE USER PERSISTENTLY
            localStorage.setItem("user", JSON.stringify(user));

            setIsAuthenticated(true);
            navigate("/");
        } else {
            const error = await response.json();
            alert(error.message || "Invalid Login ID or Password ❌");
        }
    } catch (err) {
        console.error("Login error:", err);
        alert("Server error. Please try again later.");
    }
};


    return (
        <div className="login-container">
            <div className="login-card">
                <h2 className="title">Welcome Back</h2>
                <p className="subtitle">Please login to continue</p>

                <form onSubmit={handleSubmit}>
                    <div className="input-group">
                        <input
                            type="text"
                            placeholder="Login ID"
                            value={loginId}
                            onChange={(e) => setLoginId(e.target.value)}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <input
                            type="password"
                            placeholder="Password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <button type="submit" className="login-btn">
                        Login
                    </button>
                </form>

                <p className="footer-text">© 2026 Secure Login System</p>
            </div>
        </div>
    );
};

export default Login;


