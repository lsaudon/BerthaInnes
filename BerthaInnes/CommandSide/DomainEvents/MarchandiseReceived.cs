namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public record MarchandiseReceived : IDomainEvent
    {
        public OrderId Id { get; }
        public int NumberColisRemaining { get; }

        public MarchandiseReceived(OrderId id, int numberColisRemaining)
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