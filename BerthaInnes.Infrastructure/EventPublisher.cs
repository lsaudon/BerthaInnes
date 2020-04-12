using System.Collections.Generic;
using BerthaInnes.Domain;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventStore _eventStore;
        private readonly IList<IEventHandler> _eventHandlers;

        public EventPublisher(IEventStore eventStore, IList<IEventHandler> eventHandlers)
        {
            _eventStore = eventStore;
            _eventHandlers = eventHandlers;
        }

        public void Publish<TEvent>(TEvent evt, int sequenceId) where TEvent : IDomainEvent
        {
            _eventStore.Add(evt, sequenceId);
            foreach (var eventHandler in _eventHandlers)
            {
                eventHandler.Handle(evt);
            }
        }
    }
}