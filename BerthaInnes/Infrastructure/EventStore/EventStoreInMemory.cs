using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInMemory : IEventStore
    {
        private readonly List<EventsWrapper> _eventWrappers = new List<EventsWrapper>();

        public List<EventsWrapper> GetAll(string aggregateId)
        {
            return _eventWrappers.Where(e => e.OrderId == aggregateId).ToList();
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            var sequenceId = GetSequenceId(eventsWrapper.OrderId);
            if (sequenceId >= eventsWrapper.SequenceId) throw new SequenceAlreadyStoredException();

            _eventWrappers.Add(eventsWrapper);
        }

        public int GetSequenceId(string aggregateId)
        {
            return _eventWrappers.Where(e => e.OrderId == aggregateId).ToList().Count;
        }
    }
}