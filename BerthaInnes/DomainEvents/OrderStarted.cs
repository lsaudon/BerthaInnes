using System.Collections.Generic;

namespace BerthaInnes.DomainEvents
{
    public struct OrderStarted : IDomainEvent
    {
        public List<Colis> ColisList { get; }
        public int NumberColis { get; }

        public OrderStarted(List<Colis> colisList, int numberColis)
        {
            ColisList = colisList;
            NumberColis = numberColis;
        }
    }
}