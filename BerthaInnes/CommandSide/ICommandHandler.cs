using BerthaInnes.Domain.CommandSide.DomainCommands;

namespace BerthaInnes.Domain.CommandSide
{
    public interface ICommandHandler<in TCommand> : IDomainCommand
        where TCommand : IDomainCommand
    {
        void Handle(TCommand command);
    }
}