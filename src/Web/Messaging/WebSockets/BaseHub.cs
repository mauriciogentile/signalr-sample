using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Solaise.Weather.Web.Messaging.WebSockets
{
    public abstract class BaseHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.Others.ClientConnected(new
            {
                Context.ConnectionId,
                User = Context.User.Identity.Name
            });
        }

        public override Task OnDisconnected()
        {
            return Clients.Others.ClientDisconnected(new
            {
                Context.ConnectionId,
                User = Context.User.Identity.Name
            });
        }
    }
}