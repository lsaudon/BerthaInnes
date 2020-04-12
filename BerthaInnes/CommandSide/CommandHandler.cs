using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Domain.CommandSide
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IPubSub _pubSub;
        private readonly IEventStore _eventStoreInMemory;

        public CommandHandler(IPubSub pubSub, IEventStore eventStoreInMemory)
        {
            _pubSub = pubSub;
            _eventStoreInMemory = eventStoreInMemory;
        }

        public void Handle(IDomainCommand domainCommand)
        {
            var orderId = new OrderId("1");

            var stream = _eventStoreInMemory.GetAll(orderId);
            var domainEvents = Order.Decide(domainCommand, stream).ToList();

            _pubSub.Publish(new EventsWrapper(orderId, domainEvents, stream.Count));
        }
    }
}