namespace BerthaInnes.QuerySide
{
    public interface IEventHandler
    {
        void Handle(EventWrapper evt);
    }
}