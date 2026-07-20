import "./ChangePassword.css"
import { Link } from "react-router-dom";
import { useState } from "react";
function validatePassword(password, newPassword) {
    if (newPassword.length < 8) {
        return "The password must have at least 8 characters";
    }
    if (password === newPassword) {
        return "Passwords must be different"
    }
    if (newPassword.length >= 8 && password !== newPassword) {
        return "";
    }
}


function ChangePassword() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [passwordError, setPasswordError] = useState("");

    function handlerRegister() {
        const result = validatePassword(password, newPassword);

        setPasswordError(result);
        return result;
    }

    async function SendData(e) {
        e.preventDefault();

        if (passwordError !== "") {
            return;
        }
        try {
            const response = await fetch("https://localhost:7076/user/change_password", {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Username: username,
                    Password: password,
                    NewPassword: newPassword
                })
            })
            if (response.ok) {
                window.location.href = "/login"
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
                        <label htmlFor="inputNewPassword" >
                            <img className="imgInput" src="../../.././Images/iconPassword.png" />
                        </label>
                        <input
                            type="password"
                            id="inputNewPassword"
                            placeholder="New password"
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}></input>
                    </div>

                    {passwordError && (<span className="error">{passwordError}</span>)}

                    <button type="submit" onClick={handlerRegister}>Reset password</button>

                    <span><Link to="/register">Don't have a account yet? Sign up</Link></span>
                </form>
            </div>
        </>
    );
}

export default ChangePassword;