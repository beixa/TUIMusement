using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TUIMusement.Cities;

namespace TUIMusement.Weather
{
    public class WeatherService : IWeatherService
    {
        private const string WeatherApiUrl = "http://api.weatherapi.com/v1/forecast.json";
        private const string WeatherApiKey = "53a1bc275a5045fdbf784446220208";
        private const int WeatherDays = 2;

        private IHttpClientFactory HttpFactory { get; set; }        

        public WeatherService(IHttpClientFactory httpFactory)
        {
            HttpFactory = httpFactory;
        }

        public async Task ProcessCitiesWeather(IEnumerable<City> cities)
        {
            var client = HttpFactory.CreateClient();

            foreach (var city in cities ?? new List<City>())
            {
                var response = await client.GetAsync($"{WeatherApiUrl}?key={WeatherApiKey}&q={city.Latitude},{city.Longitude}&days={WeatherDays}");
                if (response.IsSuccessStatusCode)
                {
                    var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();
                    Console.WriteLine($"Processed city {city.Code} | {weather?.Forecast?.ForecastDay?.FirstOrDefault().Day?.Condition?.Text} - {weather?.Forecast?.ForecastDay?.LastOrDefault().Day?.Condition?.Text}");
                }
                else
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(msg);
                    break;
                }                             
            }
        }
    }
}
