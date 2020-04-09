using BerthaInnes.CommandSide.DomainEvents;

namespace BerthaInnes.QuerySide
{
    public class EventWrapper
    {
        public EventWrapper(string aggregateId, IDomainEvent domainEvent)
        {
            AggregateId = aggregateId;
            DomainEvent = domainEvent;
        }

        public string AggregateId { get; }
        public IDomainEvent DomainEvent { get; }
    }
}