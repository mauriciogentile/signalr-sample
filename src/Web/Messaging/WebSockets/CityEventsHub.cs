using System;
using Microsoft.AspNet.SignalR.Hubs;
using Solaise.Weather.Domain.Events;

namespace Solaise.Weather.Web.Messaging.WebSockets
{
    [HubName("cityEventsHub")]
    public class CityEventsHub : BaseHub
    {
        readonly ICityEventsPublisher _cityEventsPublisher;

        public CityEventsHub()
            : this(CityEventsPublisher.Instance)
        {
        }

        public CityEventsHub(ICityEventsPublisher cityEventsPublisher)
        {
            _cityEventsPublisher = cityEventsPublisher;
        }

        public void CityAdded(CityAdded @event)
        {
            _cityEventsPublisher.CityAdded(new WebSocketEvent<CityAdded>(@event)
            {
                ConnectionId = Context.ConnectionId,
            });
        }

        public void CityRemoved(CityRemoved @event)
        {
            _cityEventsPublisher.CityRemoved(new WebSocketEvent<CityRemoved>(@event)
            {
                ConnectionId = Context.ConnectionId,
            });
        }
    }
}