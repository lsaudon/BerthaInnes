using System.Collections.Generic;

namespace BerthaInnes.Domain.CommandSide.DomainCommands
{
    public struct StartOrder : IDomainCommand
    {
        public List<Colis> ColisList { get; }

        public StartOrder(List<Colis> colisList)
        {
            ColisList = colisList;
        }
    }
}