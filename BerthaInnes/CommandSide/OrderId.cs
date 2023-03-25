namespace BerthaInnes.Domain.CommandSide
{
    public record OrderId : IAggregateId
    {
        public string Value { get; }

        public OrderId(string value)
        {
            Value = value;
        }
    }
}