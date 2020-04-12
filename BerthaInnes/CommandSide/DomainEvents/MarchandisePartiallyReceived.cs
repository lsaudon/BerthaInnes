namespace BerthaInnes.Domain.CommandSide.DomainEvents
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