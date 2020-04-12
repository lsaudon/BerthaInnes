﻿using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public struct EventsWrapper
    {
        public EventsWrapper(IAggregateId aggregateId, List<IDomainEvent> domainEvents, int sequenceId)
        {
            AggregateId = aggregateId;
            DomainEvents = domainEvents;
            SequenceId = sequenceId;
        }

        public IAggregateId AggregateId { get; }
        public List<IDomainEvent> DomainEvents { get; }
        public int SequenceId { get; }
    }
}