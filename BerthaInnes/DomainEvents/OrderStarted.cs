namespace BerthaInnes.DomainEvents
{
    public struct OrderStarted : IDomainEvent
    {
        public int NumberColis { get; }

        public OrderStarted(int numberColis)
        {
            NumberColis = numberColis;
        }
    }
}