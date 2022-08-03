using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TUIMusement.Cities
{
    public class CitiesService : ICitiesService
    {
        private const string MusementCitiesUrl = "https://sandbox.musement.com/api/v3/cities";

        private readonly IHttpClientFactory HttpFactory;       

        public CitiesService(IHttpClientFactory httpFactory)
        {
            HttpFactory = httpFactory;
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            var client = HttpFactory.CreateClient();

            var response = await client.GetAsync(MusementCitiesUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<City>>();
            }
            else
            {
                var msg = await response.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                return default;
            }
        }
    }
}
