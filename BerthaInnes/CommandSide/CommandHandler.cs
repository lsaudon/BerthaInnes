using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Domain.CommandSide
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStoreInMemory;

        public CommandHandler(IEventPublisher eventPublisher, IEventStore eventStoreInMemory)
        {
            _eventPublisher = eventPublisher;
            _eventStoreInMemory = eventStoreInMemory;
        }

        public void Handle(IDomainCommand domainCommand)
        {
            var orderId = new OrderId("1");

            var stream = _eventStoreInMemory.GetAll(orderId);
            var domainEvents = Order.Decide(domainCommand, stream).ToList();

            _eventPublisher.Publish(new EventsWrapper(orderId, domainEvents, stream.Count));
        }
    }
}