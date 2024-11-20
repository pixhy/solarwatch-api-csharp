import sunrise from '../assets/sunrisefeher.png';
import sunset from '../assets/sunsetfeher2.png';
import jobb from '../assets/jobb.png'
import bal from '../assets/bal.png'
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
 

function HomePage(){

  let navigate = useNavigate();
  let newDate = new Date();
  let year = newDate.getFullYear();
  let month = newDate.getMonth() + 1;  
  let day = newDate.getDate().toString().padStart(2, "0");
  let currentDate = `${year}-${month}-${day}`;

  const {setSearchedCity, searchedCity, token} = useOutletContext();
  const [date, setDate] = useState(currentDate);
  const [currentCity, setCurrentCity] = useState(null);
  const [searchHistory, setSearchHistory] = useState(null);



  function handleClick(direction){
    let moveDate;

    if (direction == "prev"){
        moveDate = new Date(Number(new Date(date)) - 60*60*24*1000) 
        
    }
    else if(direction == "next"){
        moveDate = new Date(Number(new Date(date)) + 60*60*24*1000) 
    }

    year = moveDate.getFullYear();
    month = moveDate.getMonth() + 1;  
    day = moveDate.getDate().toString().padStart(2, "0");
    const movedDate = `${year}-${month}-${day}`;
    setDate(movedDate)
  }

  useEffect(() => {
    if(!date) return;
    const url = `/api/v1/SunriseAndSunset?city=${searchedCity}&date=${date}`
  
    async function fetchSunriseAndSunset() {
      const response = await fetch(url, {
        headers: {
          "Authorization": `Bearer ${token}` 
        }
      });
      console.log(response);
      if (response.ok) {
        const data = await response.json();
        setCurrentCity(data);
        console.log(data)
      }
    }
    fetchSunriseAndSunset();

  },[date, searchedCity])

  useEffect(() => {

    const url = `/api/v1/SunriseAndSunset/UserHistory`

    async function fetchSearchHistory(){
      if(token == null){
        return 
      }

      const response = await fetch(url, {
        headers: {
          "Authorization": `Bearer ${token}` 
        }
      });
      if (response.ok) {
        const data = await response.json();
        setSearchHistory(data);
        console.log(data)
      }
    }
    fetchSearchHistory()
    
  },[currentCity])

  if(!currentCity){
    return(<div>Loading..</div>)
  }

  return (
      <>
      <div className="main-container">
          <div className="city-name">
              <div>
                {currentCity.city.name}
                <div className="country-name">
                    {currentCity.city.country}
                </div>
              </div>
          </div>

          <div className="city-sunrisesunset">sunrise and sunset
              <div className='time-data'>
                  <div>
                      <img src={sunrise}/> {currentCity.sunrise}
                  </div>
                  <div>
                      <img src={sunset}/> {currentCity.sunset}
                  </div>
              </div>
          </div>
          <div className="select-date">
            <div>
                <div>date</div>
                <div className="date-picker">
            
                <img src={bal} onClick={()=> handleClick('prev')} className='arrow'/>
                <input type="date" onChange={(e) => setDate(e.target.value)} value={date} />
                <img src={jobb} onClick={()=> handleClick('next')} className='arrow'/>
            </div>
          </div>
          </div>
          <div className="search-history">search history
            {token && searchHistory ? <>{searchHistory.map(s => <div className='user-search-history' onClick={() => setSearchedCity(s) }>{s}</div>)}</> : <div className='login-for-history'>You have to login to view your search history.</div>}
          </div>
          
      </div>
      </>
  )
}
import { useOutletContext } from 'react-router-dom';

export default HomePage;