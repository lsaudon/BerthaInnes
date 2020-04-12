using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure;
using BerthaInnes.Infrastructure.EventStore;
using Xunit;

namespace BerthaInnes.IntegrationTests
{
    public class WaitingOrderTests
    {
        [Fact]
        public void Should_Display_Updated_Projection_When_Send_Command()
        {
            var repository = new List<WaitingOrder>();
            IEventHandler pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            IList<IEventHandler> eventHandlers = new List<IEventHandler> {pendingOrderEventHandler};

            var eventStore = new EventStoreInMemory();
            var eventPublisher = new EventPublisher(eventStore, eventHandlers);
            var commandHandler = new CommandHandler(eventPublisher, eventStore);

            var colisList = new List<Colis> {new Colis()};
            commandHandler.Handle(new StartOrder(new OrderId("1"), colisList));

            Assert.Single(repository);
            Assert.Equal(new OrderId("1"), repository.First().Id);
            Assert.Equal(1, repository.First().NumberColis);
        }
    }
}