using BerthaInnes.Domain.CommandSide.DomainCommands;

namespace BerthaInnes.Domain.CommandSide
{
    public interface ICommandHandler
    {
        void Handle(IDomainCommand domainCommand);
    }
}