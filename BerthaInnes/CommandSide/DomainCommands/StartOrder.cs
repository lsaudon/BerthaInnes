using System.Collections.Generic;

namespace BerthaInnes.Domain.CommandSide.DomainCommands
{
    public struct StartOrder : IDomainCommand
    {
        public OrderId Id { get; }
        public List<Colis> ColisList { get; }

        public StartOrder(OrderId id, List<Colis> colisList)
        {
            Id = id;
            ColisList = colisList;
        }
    }
}