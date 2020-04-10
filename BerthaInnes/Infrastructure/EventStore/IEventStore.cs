using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public interface IEventStore
    {
        List<IDomainEvent> GetAll(string aggregateId);
        void Clear(string aggregateId);
        void Add(EventsWrapper eventsWrapper);
        int GetSequenceId(string aggregateId);
    }
}