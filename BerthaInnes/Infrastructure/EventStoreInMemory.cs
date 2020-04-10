using System;
using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure
{
    public interface IEventStore
    {
        List<EventsWrapper> GetAll(string aggregateId);
        void Add(EventsWrapper eventsWrapper);
        int GetSequenceId(string aggregateId);
    }

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

    public class SequenceAlreadyStoredException : Exception
    {
    }
}