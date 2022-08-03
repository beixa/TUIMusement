using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TUIMusement.Cities;
using TUIMusement.Weather;

namespace TUIMusement
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<ICitiesService, CitiesService>()
                .AddSingleton<IWeatherService, WeatherService>()
                .BuildServiceProvider();

            var citiesService = serviceProvider.GetService<ICitiesService>();
            var weatherService = serviceProvider.GetService<IWeatherService>();
            
            var cities = await citiesService.GetCities();
            await weatherService.ProcessCitiesWeather(cities);
        }
    }
}
