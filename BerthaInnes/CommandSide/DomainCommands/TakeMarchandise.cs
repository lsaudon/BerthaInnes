using System.Collections.Generic;

namespace BerthaInnes.DomainCommands
{
    public struct TakeMarchandise : IDomainCommand
    {
        public List<Colis> ColisList { get; }

        public TakeMarchandise(List<Colis> colisList)
        {
            ColisList = colisList;
        }
    }
}