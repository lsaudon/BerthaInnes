namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public interface IDomainEvent
    {
        object GetAggregateId();
    }
}