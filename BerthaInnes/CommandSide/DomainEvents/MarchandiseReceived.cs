namespace BerthaInnes.CommandSide.DomainEvents
{
    public struct MarchandiseReceived : IDomainEvent
    {
        public int NumberColisRemaining { get; }

        public MarchandiseReceived(int numberColisRemaining)
        {
            NumberColisRemaining = numberColisRemaining;
        }
    }
}