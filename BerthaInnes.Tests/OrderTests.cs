using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;
using Xunit;

namespace BerthaInnes.Tests
{
    public class OrderTests
    {
        [Fact]
        public void When_StartOrder_Then_raise_OrderStarted()
        {
            var aggregate = new Order();
            var domainEvent = aggregate.Decide(new StartOrder());
            Assert.Equal(typeof(OrderStarted),domainEvent.GetType());
        }

        [Fact]
        public void Given_OrderStarted_When_send_TakeMarchandise_Then_raise_MarchandiseReceived()
        {
            var aggregate = new Order();
            aggregate.Decide(new StartOrder());

            var domainEvent = aggregate.Decide(new TakeMarchandise());

            Assert.Equal(typeof(MarchandiseReceived),domainEvent.GetType());
        }

        [Fact]
        public void When_send_TakeMarchandise_Then_raise_MarchandiseReceived()
        {
            var aggregate = new Order();

            var domainEvent = aggregate.Decide(new TakeMarchandise());

            Assert.Equal(typeof(Nothing),domainEvent.GetType());
        }

        [Fact]
        public void Given_OrderWithMarchandiseReceived_When_TakeMarchandise_Then_raise_Nothing()
        {
            var aggregate = new Order();
            aggregate.Decide(new StartOrder());
            aggregate.Decide(new TakeMarchandise());

            var domainEvent = aggregate.Decide(new TakeMarchandise());

            Assert.Equal(typeof(Nothing),domainEvent.GetType());
        }
    }
}
