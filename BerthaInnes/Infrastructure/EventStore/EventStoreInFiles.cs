using System;
using System.Collections.Generic;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInFiles : IEventStore
    {
        public List<EventsWrapper> GetAll(string aggregateId)
        {
            throw new NotImplementedException();
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            throw new NotImplementedException();
        }

        public int GetSequenceId(string aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}