using System.Collections.Generic;
using BerthaInnes.Domain;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure
{
    public class EventPublisher : IEventPublisher
    {
        private readonly List<EventsWrapper> _eventStore;
        private readonly List<IEventHandler> _eventHandlers;

        public EventPublisher(List<EventsWrapper> eventStore, List<IEventHandler> eventHandlers)
        {
            _eventStore = eventStore;
            _eventHandlers = eventHandlers;
        }

        public void Publish(EventsWrapper eventsWrapper)
        {
            _eventStore.Add(eventsWrapper);
            foreach (var eventHandler in _eventHandlers)
            {
                eventHandler.Handle(eventsWrapper);
            }
        }
    }
}