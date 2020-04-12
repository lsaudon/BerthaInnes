using BerthaInnes.Domain.CommandSide.DomainCommands;

namespace BerthaInnes.Domain.CommandSide
{
    public class CommandHandler :
        ICommandHandler<StartOrder>,
        ICommandHandler<TakeMarchandise>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStoreInMemory;

        public CommandHandler(IEventPublisher eventPublisher, IEventStore eventStoreInMemory)
        {
            _eventPublisher = eventPublisher;
            _eventStoreInMemory = eventStoreInMemory;
        }

        public void Handle(StartOrder command)
        {
            var stream = _eventStoreInMemory.GetAll(command.Id);

            var domainEvents = Order.Decide(command, stream);

            foreach (var domainEvent in domainEvents)
            {
                _eventPublisher.Publish(domainEvent, stream.Count);
            }
        }

        public void Handle(TakeMarchandise command)
        {
            var stream = _eventStoreInMemory.GetAll(command.Id);
            var domainEvents = Order.Decide(command, stream);

            foreach (var domainEvent in domainEvents)
            {
                _eventPublisher.Publish(domainEvent, stream.Count);
            }
        }
    }
}