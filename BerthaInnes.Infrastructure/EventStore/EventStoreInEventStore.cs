﻿using System;
using System.Collections.Generic;
using System.Text;
using BerthaInnes.Domain;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInEventStore : IEventStore
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings =
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

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
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(Encoding.UTF8.GetString(evt.Event.Data),
                    _jsonSerializerSettings);
                if (domainEvent == null) continue;

                domainEvents.Add(domainEvent);
            }

            return domainEvents;
        }

        public void Clear(IAggregateId aggregateId)
        {
            throw new NotImplementedException();
        }

        public void Add(IDomainEvent domainEvent, int sequenceId)
        {
            var connection = Connection();

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings));
            var metadata = Encoding.UTF8.GetBytes("{}");
            var eventPayload = new EventData(Guid.NewGuid(), "event-type", true, data, metadata);

            List<EventData> eventDatas = new List<EventData> { eventPayload };

            connection.AppendToStreamAsync("stream-order", ExpectedVersion.Any, eventDatas).Wait();
            connection.Close();
        }
    }
}