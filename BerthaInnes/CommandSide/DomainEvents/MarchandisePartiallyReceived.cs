namespace BerthaInnes.DomainEvents
{
    public struct MarchandisePartiallyReceived : IDomainEvent
    {
        public int NumberColisRemaining { get; }

        public MarchandisePartiallyReceived(int numberColisRemaining)
        {
            NumberColisRemaining = numberColisRemaining;
        }
    }
}