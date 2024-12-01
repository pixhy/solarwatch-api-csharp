/* eslint-disable no-unused-vars */
import {   useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useOutletContext } from 'react-router-dom';


function Register(){

    const navigate = useNavigate();
    const [errorMessage, setErrorMessage] = useState(null)
    const {token, setToken} = useOutletContext();
    

    function handleSubmit(e){

        e.preventDefault();
        const username = e.target.elements.username.value;
        const email = e.target.elements.email.value;
        const password = e.target.elements.password.value;
        const passwordCheck = e.target.elements.passwordcheck.value;

        if(password != passwordCheck) {
            setErrorMessage("The passwords do not match, try again.")
            return
        }

        const requestOptions = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, email, password }),
        };


        async function registerUser() {
            const response = await fetch('/api/Auth/Register', requestOptions);
            if (response.ok) {
              const data = await response.json();
              console.log(data.token);
              setToken(data.token)
              setErrorMessage(null)
              navigate('/');
            }
        }
        registerUser();
    }
    

    return (
        <div className="login-container">
            <div className="login-inner">
                <form className="login-form" onSubmit={handleSubmit}>
                    <div>
                        <label>Email</label>
                    </div>
                    <div>
                        <input name="email" autoComplete="off"/>
                    </div>
                    <div>
                        <label>Username</label>
                    </div>
                    <div>
                        <input name="username" autoComplete="off"/>
                    </div>
                    <div>
                        <label>Password</label>
                    </div>
                    <div>
                        <input name="password" type="password"/>
                    </div>
                    <div>
                        <label>Password again</label>
                    </div>
                    <div>
                        <input name="passwordcheck" type="password"/>
                    </div>
                    <div>
                        <button className="login-button">register</button>
                    </div>
                </form>
                {errorMessage ? <div>{errorMessage}</div> : <></>}
            </div>
        </div>
    )
}

export default Register;