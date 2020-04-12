namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public interface IDomainEvent
    {
        IAggregateId GetAggregateId();
    }
}