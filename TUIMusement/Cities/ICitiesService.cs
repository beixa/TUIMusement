using System.Collections.Generic;
using System.Threading.Tasks;
using static TUIMusement.Cities.CitiesService;

namespace TUIMusement.Cities
{
    public interface ICitiesService
    {
        Task<IEnumerable<City>> GetCities();
    }
}
