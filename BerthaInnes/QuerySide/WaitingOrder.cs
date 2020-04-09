namespace BerthaInnes.QuerySide
{
    public struct WaitingOrder
    {
        public string Id { get; }

        public WaitingOrder(string id)
        {
            Id = id;
        }
    }
}