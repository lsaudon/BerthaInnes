using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public struct EventsWrapper
    {
        public EventsWrapper(string orderId, List<IDomainEvent> domainEvents, int sequenceId)
        {
            OrderId = orderId;
            DomainEvents = domainEvents;
            SequenceId = sequenceId;
        }

        public string OrderId { get; }
        public List<IDomainEvent> DomainEvents { get; }
        public int SequenceId { get; }
    }
}