using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure;
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
            var pubSub = new PubSub(eventStore, eventHandlers);

            var colisList = new List<Colis> { new Colis() };

            var commandHandler = new CommandHandler(pubSub);
            commandHandler.Handle(new StartOrder(colisList));

            Assert.Single(repository);
            Assert.Equal("1", repository.First().Id);
            Assert.Equal(1, repository.First().NumberColis);
        }
    }
}
