using System;

namespace Solaise.Weather.Web.Messaging.WebSockets
{
    public class WebSocketEvent<T>
    {
        public WebSocketEvent(T payload)
        {
            Payload = payload;
        }

        public string ConnectionId { get; set; }
        public T Payload { get; private set; }
    }
}