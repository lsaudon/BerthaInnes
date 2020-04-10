using System.Collections.Generic;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure
{
    public class PubSub
    {
        private readonly List<EventWrapper> _eventStore;
        private readonly List<IEventHandler> _eventHandlers;

        public PubSub(List<EventWrapper> eventStore, List<IEventHandler> eventHandlers)
        {
            _eventStore = eventStore;
            _eventHandlers = eventHandlers;
        }

        public void Publish(EventWrapper eventWrapper)
        {
            _eventStore.Add(eventWrapper);
            foreach (var eventHandler in _eventHandlers)
            {
                eventHandler.Handle(eventWrapper);
            }
        }
    }
}