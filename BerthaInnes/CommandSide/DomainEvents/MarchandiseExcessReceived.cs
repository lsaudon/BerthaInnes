namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public struct MarchandiseExcessReceived : IDomainEvent
    {
        public int NumberColisExcess { get; }

        public MarchandiseExcessReceived(int numberColisExcess)
        {
            NumberColisExcess = numberColisExcess;
        }
    }
}