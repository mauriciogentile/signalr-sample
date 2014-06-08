namespace Solaise.Weather.Domain.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
    }

    public class City : Entity
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}