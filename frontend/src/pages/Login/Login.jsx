import "./Login.css"
import { Link } from "react-router-dom";
import { useState } from "react";

function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    async function SendDatalogin(e) {
        e.preventDefault();

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
                localStorage.setItem("token", data.token);
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
            <form id="form" onSubmit={SendDatalogin}>
                {/*<h1>Login</h1>*/}
                <img id="formImg" src="../../.././Images/login.png"/>
                <div className="organizeInput">
                    <label htmlFor="inputUsername">
                        <img className="imgInput" src="../../.././Images/iconUser.png"/>
                    </label>
                    <input
                        type="text"
                        id="inputUsername"
                        placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value) }></input>
                </div>

                <div className="organizeInput">
                    <label htmlFor="inputPassword" >
                        <img className="imgInput" src="../../.././Images/iconPassword.png"/>
                    </label>
                    <input
                        type="password"
                        id="inputPassword"
                        placeholder="Password"
                        value={password}
                        onChange={(e) =>  setPassword(e.target.value)}></input>
                  </div>

                <button type="submit">Entrar</button>

                <span><Link to="">Forgot your password?</Link></span>
                <span><Link to="/register">Don't have an account yet? Sign up</Link></span>
            </form>
        </div>      
      </>
  );
}

export default Login;