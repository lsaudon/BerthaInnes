using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInMemory : IEventStore
    {
        private readonly Dictionary<IAggregateId, List<IDomainEvent>> _dictionary = new Dictionary<IAggregateId, List<IDomainEvent>>();

        public List<IDomainEvent> GetAll(string aggregateId)
        {
            if (_dictionary.ContainsKey(new OrderId(aggregateId)))
            {
                return _dictionary[new OrderId(aggregateId)];
            }

            return new List<IDomainEvent>();
        }

        public void Clear(string aggregateId)
        {
            _dictionary.Remove(new OrderId(aggregateId));
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            if (_dictionary.ContainsKey(new OrderId(eventsWrapper.AggregateId)))
            {
                var domainEvents = _dictionary[new OrderId(eventsWrapper.AggregateId)];
                var sequenceId = domainEvents.Count;
                if (sequenceId >= eventsWrapper.SequenceId) throw new SequenceAlreadyStoredException();
                domainEvents.Add(eventsWrapper.DomainEvents.First());
            }
            else
            {
                _dictionary.Add(new OrderId(eventsWrapper.AggregateId), new List<IDomainEvent> { eventsWrapper.DomainEvents.First() });
            }
        }

        public int GetSequenceId(string aggregateId)
        {
            return _dictionary[new OrderId(aggregateId)].Count;
        }
    }

    public interface IAggregateId
    {
        string Value { get; }
    }

    public struct OrderId : IAggregateId
    {
        public string Value { get; }

        public OrderId(string value)
        {
            Value = value;
        }
    }
}