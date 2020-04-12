using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure.EventStore;
using Xunit;

namespace BerthaInnes.Tests.Infrastructure.EventStore
{
    public class EventStoreInFilesTests
    {
        [Fact]
        public void
            Should_Return_All_Events_When_Get_All_Events_Of_Aggregate_Instance_After_Store_Events_Of_An_Aggregate_Instance()
        {
            var eventStore = new EventStoreInFiles();

            eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new OrderStarted() }, 1));
            eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new MarchandiseReceived() }, 2));

            var events = eventStore.GetAll(new OrderId("1"));

            Assert.Equal(2, events.Count);
            eventStore.Clear(new OrderId("1"));
        }

        [Fact]
        public void Should_Return_only_events_of_aggregate_instance_when_get_all_events_of_aggregate_instance_after_store_events_of_several_aggregate_instances()
        {
            var eventStore = new EventStoreInFiles();

            eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new OrderStarted() }, 1));
            eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new MarchandiseReceived() }, 2));
            eventStore.Add(new EventsWrapper(new OrderId("2"), new List<IDomainEvent> { new OrderStarted() }, 1));

            var events = eventStore.GetAll(new OrderId("1"));

            Assert.Equal(2, events.Count);
            eventStore.Clear(new OrderId("1"));
            eventStore.Clear(new OrderId("2"));
        }

        [Fact]
        public void Should_throw_when_store_event_with_sequence_event_already_stored()
        {
            var eventStore = new EventStoreInFiles();

            eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new OrderStarted() }, 1));
            eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new MarchandiseReceived() }, 2));

            Assert.Throws<SequenceAlreadyStoredException>(()
                => eventStore.Add(new EventsWrapper(new OrderId("1"), new List<IDomainEvent> { new MarchandiseReceived() }, 2)));

            eventStore.Clear(new OrderId("1"));
        }
    }
}