import { useEffect, useState } from 'react'
import './App.css'
import Header from './components/Header.jsx'
import { Outlet } from 'react-router-dom';

function App() {

  const [searchedCity, setSearchedCity] = useState(null);


  useEffect(()=> {

    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth() + 1;  
    const day = date.getDate();
    const currentDate = `${year}-${month}-${day}`;

    const url = `/api/v1/SunriseAndSunset?city=Budapest&date=${currentDate}`
  
    async function fetchSunriseAndSunset() {
      const response = await fetch(url);
      if (response.ok) {
        const data = await response.json();
        setSearchedCity(data);
        console.log(data)
      }
    }
    fetchSunriseAndSunset();
  }, []);

  return (
    <>
      <Header searchedCity={searchedCity} setSearchedCity={setSearchedCity} />
      <Outlet context= {[searchedCity]} />
    </>
  )
}


export default App
