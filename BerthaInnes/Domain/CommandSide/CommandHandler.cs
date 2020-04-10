using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure;

namespace BerthaInnes.Domain.CommandSide
{
    public interface ICommandHandler
    {
        void Handle(IDomainCommand domainCommand);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly PubSub _pubSub;

        public CommandHandler(PubSub pubSub)
        {
            _pubSub = pubSub;
        }

        public void Handle(IDomainCommand domainCommand)
        {
            var domainEvents = Order.Decide(domainCommand, new List<IDomainEvent>()).ToList();

            foreach (var domainEvent in domainEvents)
            {
                _pubSub.Publish(new EventWrapper("1", domainEvent));
            }
        }
    }
}