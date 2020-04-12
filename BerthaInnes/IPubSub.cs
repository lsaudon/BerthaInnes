using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Domain
{
    public interface IPubSub
    {
        void Publish(EventsWrapper eventsWrapper);
    }
}