using System.Collections.Generic;

namespace BerthaInnes.DomainCommands
{
    public struct StartOrder : DomainCommand
    {
        public List<Colis> ColisList { get; }

        public StartOrder(List<Colis> colisList)
        {
            ColisList = colisList;
        }
    }
}