using System.Collections.Generic;
using System.IO;
using System.Linq;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using Newtonsoft.Json;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInFiles : IEventStore
    {
        private const string _folder = "EventStore";
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public EventStoreInFiles()
        {
            Directory.CreateDirectory(_folder);
        }

        public List<IDomainEvent> GetAll(string aggregateId)
        {
            var file = File.ReadAllLines(GetFileName(aggregateId));
            var domainEvents = new List<IDomainEvent>();
            foreach (var line in file)
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(line, _jsonSerializerSettings);

                if (domainEvent == null) continue;

                domainEvents.Add(domainEvent);
            }
            return domainEvents;
        }

        public void Clear(string aggregateId)
        {
            File.Delete(GetFileName(aggregateId));
        }

        public void Add(EventsWrapper eventsWrapper)
        {
            var sequenceId = GetSequenceId(eventsWrapper.AggregateId);
            if (sequenceId >= eventsWrapper.SequenceId) throw new SequenceAlreadyStoredException();

            var lines = eventsWrapper.DomainEvents
                .Select(domainEvent => JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings));

            File.AppendAllLines(GetFileName(eventsWrapper.AggregateId), lines);
        }

        public int GetSequenceId(string aggregateId)
        {
            return File.Exists(GetFileName(aggregateId)) 
                ? File.ReadAllLines(GetFileName(aggregateId)).Length 
                : 0;
        }

        private static string GetFileName(string aggregateId)
        {
            return @$"{_folder}\{aggregateId}.txt";
        }
    }
}