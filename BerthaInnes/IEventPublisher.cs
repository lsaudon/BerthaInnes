using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent evt, int sequenceId) where TEvent : IDomainEvent;
    }
}