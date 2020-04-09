using System.Collections.Generic;

namespace BerthaInnes.DomainCommands
{
    public struct TakeMarchandise : DomainCommand
    {
        public List<Colis> ColisList { get; }

        public TakeMarchandise(List<Colis> colisList)
        {
            ColisList = colisList;
        }
    }
}