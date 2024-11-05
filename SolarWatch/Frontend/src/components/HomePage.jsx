import sunrise from '../assets/sunrisefeher.png';
import sunset from '../assets/sunsetfeher2.png';
 

function HomePage(){

   const [searchedCity] = useOutletContext();


    if(!searchedCity){
        return(<div>Loading..</div>)
    }

    return (
        <>
        <div className="main-container">
            <div className="suggestions">suggestions</div>
            <div className="city-name">
                {searchedCity.city.name}
                
            </div>
            <div className="city-sunrisesunset">sunrise and sunset
                <div className='time-data'>
                    <div>
                        <img src={sunrise}/> {searchedCity.sunrise}
                    </div>
                    <div>
                        <img src={sunset}/> {searchedCity.sunset}
                    </div>
                </div>
            </div>
            <div className="select-date">date</div>
            <div className="search-history">search history</div>
        </div>
        </>
    )
}
import { useOutletContext } from 'react-router-dom';

export default HomePage;