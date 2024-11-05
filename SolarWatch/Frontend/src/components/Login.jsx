
import { Link } from "react-router-dom";

function Login(){


    return (
        <div className="login-container">
            <div className="login-inner">
                <form className="login-form">
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
                <div className="sign-up-text">Don’t have an account? <Link to="/register">Sign up</Link></div>
            </div>
        </div>
    )
}

export default Login;