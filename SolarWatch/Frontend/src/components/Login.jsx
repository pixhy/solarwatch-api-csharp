
import { Link, useNavigate, useOutletContext } from "react-router-dom";
import { useState } from 'react';

function Login(){

    const navigate = useNavigate();
    const [errorMessage, setErrorMessage] = useState(null)
    const {token, setToken} = useOutletContext();
    

    function handleSubmit(e){

        e.preventDefault();
        const username = e.target.elements.username.value;
        const password = e.target.elements.password.value;

        const requestOptions = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password }),
        };


        async function loginUser() {
            const response = await fetch('/api/Auth/Login', requestOptions);
            if (response.ok) {
              const data = await response.json();
              console.log(data.token);
              setToken(data.token)
              setErrorMessage(null)
              navigate('/');
            }
            else {
                setErrorMessage("Username or password incorrect")
            }
        }
        loginUser();
    }


    return (
        <div className="login-container">
            <div className="login-inner">
                <form className="login-form" onSubmit={handleSubmit}>
                    <div>
                        <label>Username</label>
                    </div>
                    <div>
                        <input name="username"/>
                    </div>
                    <div>
                        <label>Password</label>
                    </div>
                    <div>
                        <input name="password"/>
                    </div>
                    <div>
                        <button className="login-button">login</button>
                    </div>
                </form>
                {errorMessage ? <div>{errorMessage}</div> : <></>}
                <div className="sign-up-text">Don’t have an account? <Link to="/register">Sign up</Link></div>
            </div>
        </div>
    )
}

export default Login;