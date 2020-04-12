namespace BerthaInnes.Infrastructure.EventStore
{
    public struct OrderId : IAggregateId
    {
        public string Value { get; }

        public OrderId(string value)
        {
            Value = value;
        }
    }
}