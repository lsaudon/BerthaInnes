using System.Collections.Generic;
using System.Linq;
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

            var domainEvents = aggregate.Decide(new StartOrder(new List<Colis>()));

            var domainEvent = domainEvents.Last();
            Assert.Equal(typeof(OrderStarted), domainEvent.GetType());
        }

        [Fact]
        public void Given_OrderStarted_When_send_TakeMarchandise_Then_raise_MarchandiseReceived()
        {
            var aggregate = new Order();
            aggregate.Decide(new StartOrder(new List<Colis>()));

            var domainEvents = aggregate.Decide(new TakeMarchandise(new List<Colis>()));

            var domainEvent = domainEvents.Last();
            Assert.Equal(typeof(MarchandiseReceived), domainEvent.GetType());
        }

        [Fact]
        public void When_send_TakeMarchandise_Then_raise_MarchandiseReceived()
        {
            var aggregate = new Order();

            var domainEvents = aggregate.Decide(new TakeMarchandise(new List<Colis>()));

            Assert.Empty(collection: domainEvents);
        }

        [Fact]
        public void Given_OrderWithMarchandiseReceived_When_TakeMarchandise_Then_raise_Nothing()
        {
            var aggregate = new Order();
            aggregate.Decide(new StartOrder(new List<Colis>()));
            aggregate.Decide(new TakeMarchandise(new List<Colis>()));

            var domainEvents = aggregate.Decide(new TakeMarchandise(new List<Colis>()));
            Assert.Empty(domainEvents);
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
            aggregate.Decide(new StartOrder(colisList));

            var domainEvents = aggregate.Decide(new TakeMarchandise(colisList.Take(5).ToList()));

            Assert.Contains(domainEvents, e => e is MarchandisePartiallyReceived);
        }

        [Fact]
        public void Given_OrderOf7Colis_With_5ColisReceived_When_TakeMarchandiseWith2Colis_Then_Raise_MarchandiseReceived()
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
            aggregate.Decide(new StartOrder(colisList));
            aggregate.Decide(new TakeMarchandise(colisList.Take(5).ToList()));

            var domainEvents = aggregate.Decide(new TakeMarchandise(colisList.Take(2).ToList()));

            Assert.Contains(domainEvents, e => e is MarchandiseReceived);
        }
    }
}
