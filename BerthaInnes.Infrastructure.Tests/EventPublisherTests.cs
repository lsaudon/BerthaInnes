using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure.EventStore;
using Xunit;

namespace BerthaInnes.Infrastructure.Tests
{
    public class EventPublisherTests
    {
        [Fact]
        public void Should_Store_Events_When_Publish_Event()
        {
            var eventStore = new EventStoreInMemory();
            var eventPublisher = new EventPublisher(eventStore, new List<IEventHandler>());

            eventPublisher.Publish(new OrderStarted(new OrderId("1"), 1), 1);

            Assert.Contains(new OrderStarted(new OrderId("1"), 1), eventStore.GetAll(new OrderId("1")));
        }

        [Fact]
        public void Should_Call_Handlers_When_Publish_Event()
        {
            var repository = new List<WaitingOrder>();
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            var eventHandlers = new List<IEventHandler> { pendingOrderEventHandler };
             
            var eventStore = new EventStoreInMemory();
            var eventPublisher = new EventPublisher(eventStore, eventHandlers);

            eventPublisher.Publish(new OrderStarted(new OrderId("1"), 1), 1);

            Assert.Single(repository);
        }
    }
}