namespace BerthaInnes.Infrastructure.EventStore
{
    public interface IAggregateId
    {
        string Value { get; }
    }
}