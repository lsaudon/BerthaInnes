using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public struct EventWrapper
    {
        public EventWrapper(string orderId, IDomainEvent domainEvent)
        {
            OrderId = orderId;
            DomainEvent = domainEvent;
        }

        public string OrderId { get; }
        public IDomainEvent DomainEvent { get; }
    }
}