using System.Collections.Generic;
using System.Linq;
using BerthaInnes.CommandSide;
using BerthaInnes.CommandSide.DomainCommands;
using BerthaInnes.CommandSide.DomainEvents;
using BerthaInnes.QuerySide;
using Xunit;

namespace BerthaInnes.IntegrationTests
{
    public class WaitingOrderTests
    {
        [Fact]
        public void Should_Display_Updated_Projection_When_Send_Command()
        {
            var repository = new List<WaitingOrder>();
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            var eventHandlers = new List<IEventHandler> { pendingOrderEventHandler };

            var eventStore = new List<EventWrapper>();
            var pubSub = new PubSub(eventStore,eventHandlers);

            var colisList = new List<Colis> { new Colis() };
            var domainEvents = Order.Decide(new StartOrder(colisList), new List<IDomainEvent>()).ToList();

            foreach (var domainEvent in domainEvents)
            {
                pubSub.Publish(new EventWrapper("1",domainEvent));
            }
            
            Assert.Single(repository);
        }
    }
}
