using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure;
using BerthaInnes.Infrastructure.EventStore;

namespace BerthaInnes.Domain.CommandSide
{
    public interface ICommandHandler
    {
        void Handle(IDomainCommand domainCommand);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly PubSub _pubSub;
        private readonly IEventStore _eventStoreInMemory;

        public CommandHandler(PubSub pubSub, IEventStore eventStoreInMemory)
        {
            _pubSub = pubSub;
            _eventStoreInMemory = eventStoreInMemory;
        }

        public void Handle(IDomainCommand domainCommand)
        {
            var domainEvents = Order.Decide(domainCommand, new List<IDomainEvent>()).ToList();

            var orderId = "1";
            var sequenceId = _eventStoreInMemory.GetSequenceId(orderId);

            _pubSub.Publish(new EventsWrapper(orderId, domainEvents,sequenceId));
        }
    }
}