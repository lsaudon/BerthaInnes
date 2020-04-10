using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure;
using Xunit;

namespace BerthaInnes.Tests
{
    public class PubSubTests
    {
        [Fact]
        public void Should_Store_Events_When_Publish_Event()
        {
            var eventStore = new List<EventWrapper>();
            var pubSub = new PubSub(eventStore, new List<IEventHandler>());

            pubSub.Publish(new EventWrapper("1", new OrderStarted(1)));

            Assert.Contains(new EventWrapper("1", new OrderStarted(1)), eventStore);
        }

        [Fact]
        public void Should_Call_Handlers_When_Publish_Event()
        {

            var repository = new List<WaitingOrder>();
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            var eventHandlers = new List<IEventHandler> { pendingOrderEventHandler };

            var eventStore = new List<EventWrapper>();
            var pubSub = new PubSub(eventStore, eventHandlers);

            pubSub.Publish(new EventWrapper("1", new OrderStarted(1)));

            Assert.Single(repository);
        }


    }
}