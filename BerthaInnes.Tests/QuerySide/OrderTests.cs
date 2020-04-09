using System.Collections.Generic;
using BerthaInnes.CommandSide.DomainEvents;
using BerthaInnes.QuerySide;
using Xunit;

namespace BerthaInnes.Tests.QuerySide
{
    public class OrderTests
    {
        [Fact]
        public void When_Receive_OrderCreated_Then_This_Order_Is_Added_In_Waiting_Orders()
        {
            var repository = new List<WaitingOrder>();
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);

            var evt = new EventWrapper("1", new OrderStarted(1));

            pendingOrderEventHandler.Handle(evt);

            Assert.Single(repository);
        }

        [Fact]
        public void When_Receive_MarchandiseReceived_Then_This_Order_Is_Removed_of_WaitingOrders()
        {
            var repository = new List<WaitingOrder> { new WaitingOrder("1") };
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);

            var evt = new EventWrapper("1", new MarchandiseReceived(1));

            pendingOrderEventHandler.Handle(evt);

            Assert.Empty(repository);
        }

        [Fact]
        public void Given_2_WaitingOrders_A_and_B_When_Receive_MarchandiseReceived_Of_Order_B_Then_I_Have_Only_Order_A_In_Waiting_Orders()
        {
            var repository = new List<WaitingOrder> { new WaitingOrder("A"), new WaitingOrder("B") };
            var pendingOrderEventHandler = new PendingOrderEventHandler(repository);

            var evt = new EventWrapper("B", new MarchandiseReceived(0));

            pendingOrderEventHandler.Handle(evt);

            Assert.Single(repository);
            Assert.Contains(repository, x => x.Id == "A");
        }
    }
}
