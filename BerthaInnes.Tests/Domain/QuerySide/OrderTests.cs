using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using Xunit;

namespace BerthaInnes.Tests.Domain.QuerySide
{
    public class OrderTests
    {
        [Fact]
        public void When_Receive_OrderCreated_Then_This_Order_Is_Added_In_Waiting_Orders()
        {
            var repository = new List<WaitingOrder>();
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);

            var domainEvents = new List<IDomainEvent> { new OrderStarted(1) };
            var evt = new EventsWrapper("1", domainEvents, 1);

            pendingOrderEventHandler.Handle(evt);

            Assert.Single(repository);
        }

        [Fact]
        public void When_Receive_MarchandiseReceived_Then_This_Order_Is_Removed_of_WaitingOrders()
        {
            var repository = new List<WaitingOrder> { new WaitingOrder("1", 1) };
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            var domainEvents = new List<IDomainEvent> { new MarchandiseReceived(1) };
            var evt = new EventsWrapper("1", domainEvents, 1);

            pendingOrderEventHandler.Handle(evt);

            Assert.Empty(repository);
        }

        [Fact]
        public void Given_2_WaitingOrders_A_and_B_When_Receive_MarchandiseReceived_Of_Order_B_Then_I_Have_Only_Order_A_In_Waiting_Orders()
        {
            var repository = new List<WaitingOrder> { new WaitingOrder("A", 1), new WaitingOrder("B", 1) };
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);
            var domainEvents = new List<IDomainEvent> { new MarchandiseReceived(0) };
            var evt = new EventsWrapper("B", domainEvents, 1);

            pendingOrderEventHandler.Handle(evt);

            Assert.Single(repository);
            Assert.Contains(repository, x => x.Id == "A");
        }
    }
}
