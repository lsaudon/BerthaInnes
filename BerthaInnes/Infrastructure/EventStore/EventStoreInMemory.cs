using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInMemory : IEventStore
    {
        private readonly Dictionary<IAggregateId, List<IDomainEvent>> _dictionary = new Dictionary<IAggregateId, List<IDomainEvent>>();

        public List<IDomainEvent> GetAll(IAggregateId aggregateId)
        {
            return _dictionary.ContainsKey(aggregateId) 
                ? _dictionary[aggregateId] 
                : new List<IDomainEvent>();
        }

        public void Clear(IAggregateId aggregateId)
        {
            _dictionary.Remove(aggregateId);
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            if (_dictionary.ContainsKey(eventsWrapper.AggregateId))
            {
                var domainEvents = _dictionary[eventsWrapper.AggregateId];
                var sequenceId = domainEvents.Count;
                if (sequenceId >= eventsWrapper.SequenceId) throw new SequenceAlreadyStoredException();

                domainEvents.AddRange(eventsWrapper.DomainEvents);
                return;
            }

            foreach (var domainEvent in eventsWrapper.DomainEvents)
            {
                _dictionary.Add(eventsWrapper.AggregateId, new List<IDomainEvent> { domainEvent });
            }
        }

        public int GetSequenceId(IAggregateId aggregateId)
        {
            return _dictionary[aggregateId].Count;
        }
    }
}