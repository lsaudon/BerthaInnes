using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Infrastructure.EventStore;

namespace BerthaInnes.Domain.QuerySide
{
    public struct WaitingOrder
    {
        public IAggregateId Id { get; }
        public int NumberColis { get; }

        public WaitingOrder(IAggregateId id, int numberColis)
        {
            Id = id;
            NumberColis = numberColis;
        }
    }
}