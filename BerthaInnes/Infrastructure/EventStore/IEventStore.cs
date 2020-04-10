using System.Collections.Generic;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public interface IEventStore
    {
        List<EventsWrapper> GetAll(string aggregateId);
        void Add(EventsWrapper eventsWrapper);
        int GetSequenceId(string aggregateId);
    }
}