namespace BerthaInnes.Domain.QuerySide
{
    public struct WaitingOrder
    {
        public string Id { get; }
        public int NumberColis { get; }

        public WaitingOrder(string id, int numberColis)
        {
            Id = id;
            NumberColis = numberColis;
        }
    }
}