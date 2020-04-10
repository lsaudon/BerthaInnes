using System.Collections.Generic;
using System.IO;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using Newtonsoft.Json;

namespace BerthaInnes.Infrastructure.EventStore
{
    public class EventStoreInFiles : IEventStore
    {
        private const string _folder = "EventStore";

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
                var jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(line, jsonSerializerSettings);
                if (domainEvent != null)
                {
                    domainEvents.Add(domainEvent);
                }
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

            foreach (var domainEvent in eventsWrapper.DomainEvents)
            {
                var jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var serialize = JsonConvert.SerializeObject(domainEvent, jsonSerializerSettings);
                File.AppendAllText(GetFileName(eventsWrapper.AggregateId), serialize + "\n");
            }
        }

        public int GetSequenceId(string aggregateId)
        {
            if (File.Exists(GetFileName(aggregateId)))
            {
                return File.ReadAllLines(GetFileName(aggregateId)).Length;
            }

            return 0;
        }

        private string GetFileName(string aggregateId)
        {
            return @$"{_folder}\{aggregateId}.txt";
        }
    }
}