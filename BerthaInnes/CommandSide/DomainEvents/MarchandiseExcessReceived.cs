﻿namespace BerthaInnes.Domain.CommandSide.DomainEvents
{
    public record MarchandiseExcessReceived : IDomainEvent
    {
        public OrderId Id { get; }
        public int NumberColisExcess { get; }

        public MarchandiseExcessReceived(OrderId id, int numberColisExcess)
        {
            Id = id;
            NumberColisExcess = numberColisExcess;
        }

        public IAggregateId GetAggregateId()
        {
            return Id;
        }
    }
}