namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public record OrderStarted : IDomainEvent
    {
        public OrderId Id { get; }
        public int NumberColis { get; }

        public OrderStarted(OrderId id, int numberColis)
        {
            Id = id;
            NumberColis = numberColis;
        }

        public IAggregateId GetAggregateId()
        {
            return Id;
        }
    }
}