namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public record MarchandisePartiallyReceived : IDomainEvent
    {
        public OrderId Id { get; }

        public int NumberColisRemaining { get; }

        public MarchandisePartiallyReceived(OrderId id, int numberColisRemaining)
        {
            Id = id;
            NumberColisRemaining = numberColisRemaining;
        }

        public IAggregateId GetAggregateId()
        {
            return Id;
        }
    }
}