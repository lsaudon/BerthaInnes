namespace BerthaInnes.Domain.QuerySide
{
    public interface IEventHandler
    {
        void Handle(EventWrapper evt);
    }
}