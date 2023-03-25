﻿using System.Collections.Generic;
using System.IO;
using BerthaInnes.Domain;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using Newtonsoft.Json;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInFiles : IEventStore
    {
        private const string _folder = "EventStore";

        private readonly JsonSerializerSettings _jsonSerializerSettings =
            new() { TypeNameHandling = TypeNameHandling.All };

        public EventStoreInFiles()
        {
            Directory.CreateDirectory(_folder);
        }

        public List<IDomainEvent> GetAll(IAggregateId aggregateId)
        {
            var file = File.ReadAllLines(GetFileName(aggregateId.Value));
            var domainEvents = new List<IDomainEvent>();
            foreach (var line in file)
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(line, _jsonSerializerSettings);

                if (domainEvent == null) continue;

                domainEvents.Add(domainEvent);
            }

            return domainEvents;
        }

        public void Clear(IAggregateId aggregateId)
        {
            File.Delete(GetFileName(aggregateId.Value));
        }

        public void Add(IDomainEvent domainEvent, int sequenceId)
        {
            if (GetSequenceId(domainEvent.GetAggregateId()) >= sequenceId) throw new SequenceAlreadyStoredException();

            var line = JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings);

            File.AppendAllLines(GetFileName(domainEvent.GetAggregateId().Value), new[] { line });
        }

        private static int GetSequenceId(IAggregateId aggregateId)
        {
            return File.Exists(GetFileName(aggregateId.Value))
                ? File.ReadAllLines(GetFileName(aggregateId.Value)).Length
                : 0;
        }

        private static string GetFileName(string aggregateId)
        {
            return @$"{_folder}\{aggregateId}.txt";
        }
    }
}