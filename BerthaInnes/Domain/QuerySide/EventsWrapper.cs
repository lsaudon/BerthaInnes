using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public struct EventsWrapper
    {
        public EventsWrapper(string aggregateId, List<IDomainEvent> domainEvents, int sequenceId)
        {
            AggregateId = aggregateId;
            DomainEvents = domainEvents;
            SequenceId = sequenceId;
        }

        public string AggregateId { get; }
        public List<IDomainEvent> DomainEvents { get; }
        public int SequenceId { get; }
    }
}