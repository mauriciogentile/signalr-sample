namespace Solaise.Weather.Domain.Events
{
    public class CityRemoved : Event<int>
    {
        public CityRemoved(int id) : base(id) { }
    }
}