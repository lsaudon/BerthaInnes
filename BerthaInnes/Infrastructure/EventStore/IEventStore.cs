using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public interface IEventStore
    {
        List<IDomainEvent> GetAll(IAggregateId aggregateId);
        void Clear(IAggregateId aggregateId);
        void Add(EventsWrapper eventsWrapper);
    }
}