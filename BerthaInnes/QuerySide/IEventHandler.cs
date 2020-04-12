using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public interface IEventHandler
    {
        void Handle(EventsWrapper evt);
    }

    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IDomainEvent
    {
        void Handle(TEvent evt);
    }
}