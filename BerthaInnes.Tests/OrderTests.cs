using System.Collections.Generic;
using System.Linq;
using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;
using NuGet.Frameworks;
using Xunit;

namespace BerthaInnes.Tests
{
    public class OrderTests
    {
        [Fact]
        public void When_StartOrder_Then_raise_OrderStarted()
        {
            var aggregate = new Order();
            var domainEvents = aggregate.Decide(command: new StartOrder());
            var domainEvent = domainEvents.Last();
            Assert.Equal(expected: typeof(OrderStarted), actual: domainEvent.GetType());
        }

        [Fact]
        public void Given_OrderStarted_When_send_TakeMarchandise_Then_raise_MarchandiseReceived()
        {
            var aggregate = new Order();
            aggregate.Decide(command: new StartOrder());

            var domainEvents = aggregate.Decide(command: new TakeMarchandise());
            var domainEvent = domainEvents.Last();
            Assert.Equal(expected: typeof(MarchandiseReceived), actual: domainEvent.GetType());
        }

        [Fact]
        public void When_send_TakeMarchandise_Then_raise_MarchandiseReceived()
        {
            var aggregate = new Order();

            var domainEvents = aggregate.Decide(command: new TakeMarchandise());
            Assert.Empty(collection: domainEvents);
        }

        [Fact]
        public void Given_OrderWithMarchandiseReceived_When_TakeMarchandise_Then_raise_Nothing()
        {
            var aggregate = new Order();
            aggregate.Decide(command: new StartOrder());
            aggregate.Decide(command: new TakeMarchandise());

            var domainEvents = aggregate.Decide(command: new TakeMarchandise());
            Assert.Empty(collection: domainEvents);
        }

        [Fact]
        public void Given_OrderStartedOf7Colis_When_TakeMarchandiseWith5Colis_Then_raise_MarchandisePartiallyReceived()
        {
            var aggregate = new Order();
            var colisList = new List<Colis>
            {
                new Colis(), 
                new Colis(),
                new Colis(), 
                new Colis(), 
                new Colis(), 
                new Colis(), 
                new Colis()
            };
            aggregate.Decide(command: new StartOrder(colisList: colisList));

            var domainEvents = aggregate.Decide(command: new TakeMarchandise());
            var domainEvent = domainEvents.Last();
            Assert.True(condition: domainEvents.Any(predicate: e => e is MarchandisePartiallyReceived));
            Assert.Equal(expected: typeof(MarchandiseReceived), actual: domainEvent.GetType());
        }
    }
}
