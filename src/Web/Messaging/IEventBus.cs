using System;

namespace Solaise.Weather.Web.Messaging
{
    public interface IEventBus : IPublisher, ISubscriber
    {
    }

    public interface IPublisher
    {
        void Publish<TEvent>(TEvent sampleEvent);
    }

    public interface ISubscriber
    {
        IDisposable Subscribe<TEvent>(Action<TEvent> handler);
    }
}