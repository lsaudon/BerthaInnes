using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure;
using BerthaInnes.Infrastructure.EventStore;
using Xunit;

namespace BerthaInnes.Tests.Infrastructure
{
    public class PubSubTests
    {
        [Fact]
        public void Should_Store_Events_When_Publish_Event()
        {
            var eventStore = new List<EventsWrapper>();
            var pubSub = new PubSub(eventStore, new List<IEventHandler>());

            var domainEvents = new List<IDomainEvent> { new OrderStarted(1) };
            pubSub.Publish(new EventsWrapper(new OrderId("1"), domainEvents, 1));

            Assert.Contains(new EventsWrapper(new OrderId("1"), domainEvents, 1), eventStore);
        }

        [Fact]
        public void Should_Call_Handlers_When_Publish_Event()
        {

            var repository = new List<WaitingOrder>();
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            var eventHandlers = new List<IEventHandler> { pendingOrderEventHandler };

            var eventStore = new List<EventsWrapper>();
            var pubSub = new PubSub(eventStore, eventHandlers);

            var domainEvents = new List<IDomainEvent> { new OrderStarted(1) };
            pubSub.Publish(new EventsWrapper(new OrderId("1"), domainEvents, 1));

            Assert.Single(repository);
        }


    }
}