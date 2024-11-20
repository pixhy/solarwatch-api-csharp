import { useEffect, useState } from 'react'
import './App.css'
import Header from './components/Header.jsx'
import { Outlet } from 'react-router-dom';

function App() {

  const [searchedCity, setSearchedCity] = useState("Budapest");
  const [token, setTokenState] = useState(localStorage.getItem("solarwatch_token"));
  function setToken(newValue){
    setTokenState(newValue);
    if(newValue !== null){
      localStorage.setItem("solarwatch_token", newValue);
    }
    else {
      localStorage.removeItem("solarwatch_token");
    }
  }

  return (
    <>
      <Header searchedCity={searchedCity} setSearchedCity={setSearchedCity} token={token} setToken={setToken} />
      <Outlet context= {{setSearchedCity, searchedCity, setToken, token}}  />
    </>
  )
}


export default App
