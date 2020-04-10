using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInMemory : IEventStore
    {
        private readonly List<EventsWrapper> _eventsWrappers = new List<EventsWrapper>();

        public List<IDomainEvent> GetAll(string aggregateId)
        {
            return _eventsWrappers.Where(e => e.AggregateId == aggregateId).SelectMany(e => e.DomainEvents).ToList();
        }

        public void Clear(string aggregateId)
        {
            _eventsWrappers.RemoveAll(e => e.AggregateId == aggregateId);
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            var sequenceId = GetSequenceId(eventsWrapper.AggregateId);
            if (sequenceId >= eventsWrapper.SequenceId) throw new SequenceAlreadyStoredException();

            _eventsWrappers.Add(eventsWrapper);
        }

        public int GetSequenceId(string aggregateId)
        {
            return _eventsWrappers.Where(e => e.AggregateId == aggregateId).ToList().Count;
        }
    }
}