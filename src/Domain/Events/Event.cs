namespace Solaise.Weather.Domain.Events
{
    public class Event<T>
    {
        public Event(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; private set; }
    }
}
