using System.Collections.Generic;
using System.Threading.Tasks;
using TUIMusement.Cities;

namespace TUIMusement.Weather
{
    public interface IWeatherService
    {
        Task ProcessCitiesWeather(IEnumerable<City> cities);
    }
}
