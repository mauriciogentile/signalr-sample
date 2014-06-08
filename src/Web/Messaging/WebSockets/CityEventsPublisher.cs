using System;
using Microsoft.AspNet.SignalR;
using Solaise.Weather.Domain.Events;

namespace Solaise.Weather.Web.Messaging.WebSockets
{
    public class CityEventsPublisher : ICityEventsPublisher
    {
        static readonly Lazy<CityEventsPublisher> _instance =
            new Lazy<CityEventsPublisher>(() => new CityEventsPublisher(GlobalHost.ConnectionManager.GetHubContext<CityEventsHub>()));

        readonly IHubContext _context;

        private CityEventsPublisher(IHubContext context)
        {
            _context = context;
        }

        public static CityEventsPublisher Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void CityAdded(WebSocketEvent<CityAdded> @event)
        {
            _context.Clients.AllExcept(@event.ConnectionId).All.cityAdded(@event.Payload);
        }

        public void CityRemoved(WebSocketEvent<CityRemoved> @event)
        {
            _context.Clients.AllExcept(@event.ConnectionId).cityRemoved(@event.Payload);
        }
    }
}