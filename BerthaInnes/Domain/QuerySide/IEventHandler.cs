namespace BerthaInnes.Domain.QuerySide
{
    public interface IEventHandler
    {
        void Handle(EventsWrapper evt);
    }
}