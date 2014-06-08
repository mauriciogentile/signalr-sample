using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Solaise.Weather.Web.Messaging
{
    public class DefaultEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public void Publish<TEvent>(TEvent eventData)
        {
            object subject;
            if (_subjects.TryGetValue(typeof(TEvent), out subject))
            {
                var observer = (ISubject<TEvent>)subject;
                observer.OnNext(eventData);
            }
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> handler)
        {
            var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());
            return subject.AsObservable().Subscribe(handler);
        }
    }
}