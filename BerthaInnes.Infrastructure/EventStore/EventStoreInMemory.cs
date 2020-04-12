using System.Collections.Generic;
using BerthaInnes.Domain;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInMemory : IEventStore
    {
        private readonly Dictionary<IAggregateId, List<IDomainEvent>> _dictionary =
            new Dictionary<IAggregateId, List<IDomainEvent>>();

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

        //public void Add(EventsWrapper eventsWrapper)
        //{
        //    if (_dictionary.ContainsKey(eventsWrapper.AggregateId))
        //    {
        //        var domainEvents = _dictionary[eventsWrapper.AggregateId];
        //        var sequenceId = domainEvents.Count;
        //        if (sequenceId >= eventsWrapper.SequenceId) throw new SequenceAlreadyStoredException();

        //        domainEvents.AddRange(eventsWrapper.DomainEvents);
        //        return;
        //    }

        //    foreach (var domainEvent in eventsWrapper.DomainEvents)
        //    {
        //        _dictionary.Add(eventsWrapper.AggregateId, new List<IDomainEvent> { domainEvent });
        //    }
        //}

        public void Add(IDomainEvent domainEvent, int sequenceId)
        {
            IAggregateId aggregateId = domainEvent.GetAggregateId();
            if (_dictionary.ContainsKey(aggregateId))
            {
                var domainEvents = _dictionary[aggregateId];
                if (domainEvents.Count >= sequenceId) throw new SequenceAlreadyStoredException();

                domainEvents.Add(domainEvent);
                return;
            }

            _dictionary.Add(aggregateId, new List<IDomainEvent> { domainEvent });
        }
    }
}