﻿using BerthaInnes.Domain.CommandSide;

namespace BerthaInnes.Domain.QuerySide
{
    public struct WaitingOrder
    {
        public IAggregateId Id { get; }
        public int NumberColis { get; }

        public WaitingOrder(IAggregateId id, int numberColis)
        {
            Id = id;
            NumberColis = numberColis;
        }
    }
}