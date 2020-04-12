using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain
{
    public interface IEventStore
    {
        List<IDomainEvent> GetAll(IAggregateId aggregateId);
        void Clear(IAggregateId aggregateId);
        void Add(IDomainEvent domainEvent, int sequenceId);
    }
}