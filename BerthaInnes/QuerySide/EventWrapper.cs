using BerthaInnes.CommandSide.DomainEvents;

namespace BerthaInnes.QuerySide
{
    public class EventWrapper
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