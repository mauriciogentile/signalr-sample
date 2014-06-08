using Solaise.Weather.Domain.Entities;

namespace Solaise.Weather.Domain.Events
{
    public class CityAdded : Event<City>
    {
        public CityAdded(City city) : base(city) { }
    }
}
