using System.Collections.Generic;

namespace BerthaInnes.Domain.CommandSide.DomainCommands
{
    public record TakeMarchandise : IDomainCommand
    {
        public OrderId Id { get; }
        public List<Colis> ColisList { get; }

        public TakeMarchandise(OrderId id, List<Colis> colisList)
        {
            Id = id;
            ColisList = colisList;
        }
    }
}