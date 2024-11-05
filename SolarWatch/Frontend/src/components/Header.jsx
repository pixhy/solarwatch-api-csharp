import solarwatchlogo from '../assets/solarwatchlogo.png'
import magnifyingglass from '../assets/fehernagyito.png'
import { useNavigate } from 'react-router-dom';

function Header({searchedCity, setSearchedCity}){


  const navigate = useNavigate();

  function handleSubmit(e){

    e.preventDefault();
    const city = e.target.searchfield.value;

    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth() + 1;  
    const day = date.getDate();
    const currentDate = `${year}-${month}-${day}`;

    const url = `/api/v1/SunriseAndSunset?city=${city}&date=${currentDate}`
  
    async function fetchSunriseAndSunset() {
      const response = await fetch(url);
      if (response.ok) {
        const data = await response.json();
        setSearchedCity(data);
        console.log(data)
      }
    }
    fetchSunriseAndSunset();
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
              <button onClick={()=> navigate("/login")} className="login">login</button>
            </div>
          </div>
        </div>
        
      );
}

export default Header;