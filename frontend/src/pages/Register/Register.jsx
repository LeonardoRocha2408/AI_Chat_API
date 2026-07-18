import "./Register.css"
import { Link } from "react-router-dom";
import { useState } from "react";


function validatePassword(password, confirmPassword) {
    if (password.length < 8) {
        return "Password has must at least 8 characters";
    }
    if (password !== confirmPassword) {
        return "Password's are different"
    }
    if (password.length >= 8 && password === confirmPassword) {
        return "";
    }
}


function Register() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [passwordError, setPasswordError] = useState("");

    function handlerRegister() {
        const result = validatePassword(password, confirmPassword);

        setPasswordError(result);
        return result;
    }

    async function SendData(e) {
        e.preventDefault();

        if (passwordError !== "") {
            return;
        }
        try {
            const response = await fetch("https://localhost:7076/user/account_login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Username: username,
                    Password: password
                })
            })
            if (response.ok) {
                window.location.href = "/chat"
                return response.json();
            }
        }
        catch (error) {
            alert(error);
        }
    }
    return (
        <>
            <div className="login-page">
                <form id="form" onSubmit={SendData}>
                    <img id="formImg" src="../../.././Images/login.png" />
                    <div className="organizeInput">
                        <label htmlFor="inputUsername">
                            <img className="imgInput" src="../../.././Images/iconUser.png" />
                        </label>
                        <input
                            type="text"
                            id="inputUsername"
                            placeholder="Username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}></input>
                    </div>

                    <div className="organizeInput">
                        <label htmlFor="inputPassword" >
                            <img className="imgInput" src="../../.././Images/iconPassword.png" />
                        </label>
                        <input
                            type="password"
                            id="inputPassword"
                            placeholder="Password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}></input>
                    </div>
                    <div className="organizeInput">
                        <label htmlFor="inputConfirmPassword" >
                            <img className="imgInput" src="../../.././Images/iconPassword.png" />
                        </label>
                        <input
                            type="password"
                            id="inputConfirmPassword"
                            placeholder="Confirm password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}></input>
                    </div>

                    {passwordError && (<span className="error">{passwordError}</span>)}

                    <button type="submit" onClick={handlerRegister}>Criar</button>

                    <span><Link to="">Forgot your password?</Link></span>
                    <span><Link to=".././login">Do you already have account? Sign in</Link></span>
                </form>
            </div>
        </>
    );
}

export default Register;