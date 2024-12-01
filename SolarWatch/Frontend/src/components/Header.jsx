import solarwatchlogo from '../assets/solarwatchlogo.png'
import magnifyingglass from '../assets/fehernagyito.png'
import { useNavigate } from 'react-router-dom';

function Header({setSearchedCity, token, setToken}){

  
  const navigate = useNavigate();

  function handleSubmit(e){

    e.preventDefault();
    const city = e.target.searchfield.value;

    setSearchedCity(city);
    e.target.searchfield.value = ""
  }

  function logout(){
    setToken(null)
  }

    return (
        <div className="header">
          <div className="header-content">
            <div className="left-side" onClick={()=> navigate("/")}>
              <img src={solarwatchlogo} />
              <button className="solarwatch">SolarWatch</button>
            </div>
            <div className="right-side">
              <img src={magnifyingglass} className="magnifying-glass"/>
              <form onSubmit={handleSubmit}>
                <input className="searchfield" placeholder="search..." name="searchfield" />
              </form>
              {!token ? <button onClick={()=> navigate("/login")} className="login">login</button> : <button onClick={logout} className="login">logout</button>}
            </div>
          </div>
        </div>
        
      );
}

export default Header;