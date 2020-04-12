using System;
using System.Collections.Generic;
using System.Text;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInEventStore : IEventStore
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        private static IEventStoreConnection Connection()
        {
            var connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));
            connection.ConnectAsync().Wait();
            return connection;
        }

        public List<IDomainEvent> GetAll(IAggregateId aggregateId)
        {
            var connection = Connection();
            var readEvents = connection.ReadStreamEventsForwardAsync("newstream", 0, 10, true).Result;
            connection.Close();

            List<IDomainEvent> domainEvents = new List<IDomainEvent>();
            foreach (var evt in readEvents.Events)
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(Encoding.UTF8.GetString(evt.Event.Data), _jsonSerializerSettings);
                if (domainEvent == null) continue;

                domainEvents.Add(domainEvent);
            }

            return domainEvents;
        }

        public void Clear(IAggregateId aggregateId)
        {
            throw new NotImplementedException();
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            var connection = Connection();

            List<EventData> eventDatas = new List<EventData>();

            foreach (var domainEvent in eventsWrapper.DomainEvents)
            {
                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings));
                var metadata = Encoding.UTF8.GetBytes("{}");
                var eventPayload = new EventData(Guid.NewGuid(), "event-type", true, data, metadata);
                eventDatas.Add(eventPayload);
            }

            connection.AppendToStreamAsync("stream-order", ExpectedVersion.Any, eventDatas).Wait();
            connection.Close();
        }

        public int GetSequenceId(IAggregateId aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}