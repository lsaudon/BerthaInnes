using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainCommands;
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
            var orderId = "1";

            var stream = _eventStoreInMemory.GetAll(orderId);
            var domainEvents = Order.Decide(domainCommand, stream).ToList();

            _pubSub.Publish(new EventsWrapper(orderId, domainEvents, stream.Count));
        }
    }
}