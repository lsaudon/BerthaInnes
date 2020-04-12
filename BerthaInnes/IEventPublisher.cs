using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Domain
{
    public interface IEventPublisher
    {
        void Publish(EventsWrapper eventsWrapper);
    }
}