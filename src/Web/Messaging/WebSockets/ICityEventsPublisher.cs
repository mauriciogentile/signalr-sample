using Solaise.Weather.Domain.Events;

namespace Solaise.Weather.Web.Messaging.WebSockets
{
    public interface ICityEventsPublisher
    {
        void CityAdded(WebSocketEvent<CityAdded> @event);
        void CityRemoved(WebSocketEvent<CityRemoved> @event);
    }
}
