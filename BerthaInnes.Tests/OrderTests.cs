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
            var aggregate = new Order(new List<IDomainEvent>());

            var domainEvents = aggregate.Decide(new StartOrder(new List<Colis>()));

            var domainEvent = domainEvents.Last();
            Assert.Equal(typeof(OrderStarted), domainEvent.GetType());
        }

        [Fact]
        public void Given_OrderStarted_When_send_TakeMarchandise_Then_Raise_MarchandiseReceived()
        {
            var events = new List<IDomainEvent> { new OrderStarted(new List<Colis>(), 0) };
            var aggregate = new Order(events);

            var domainEvents = aggregate.Decide(new TakeMarchandise(new List<Colis>()));

            Assert.Contains(domainEvents, e => e is MarchandiseReceived);
        }

        [Fact]
        public void When_Send_TakeMarchandise_Then_Raise_MarchandiseReceived()
        {
            var aggregate = new Order(new List<IDomainEvent>());

            var domainEvents = aggregate.Decide(new TakeMarchandise(new List<Colis>()));

            Assert.Empty(domainEvents);
        }

        [Fact]
        public void Given_OrderWithMarchandiseReceived_When_TakeMarchandise_Then_Raise_Nothing()
        {
            var aggregate = new Order(new List<IDomainEvent>());
            aggregate.Decide(new StartOrder(new List<Colis>()));
            aggregate.Decide(new TakeMarchandise(new List<Colis>()));

            var domainEvents = aggregate.Decide(new TakeMarchandise(new List<Colis>()));
            Assert.Empty(domainEvents);
        }

        [Fact]
        public void Given_OrderStartedOf7Colis_When_TakeMarchandiseWith5Colis_Then_Raise_MarchandisePartiallyReceived()
        {
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
            var events = new List<IDomainEvent> { new OrderStarted(colisList, 7) };
            var aggregate = new Order(events);

            var domainEvents = aggregate.Decide(new TakeMarchandise(colisList.Take(5).ToList()));

            Assert.Contains(domainEvents, e => e is MarchandisePartiallyReceived);
        }

        [Fact]
        public void Given_OrderOf7Colis_With_5ColisReceived_When_TakeMarchandiseWith2Colis_Then_Raise_MarchandiseReceived()
        {
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
            var events = new List<IDomainEvent>
            {
                new OrderStarted(colisList,7),
                new MarchandisePartiallyReceived(colisList.Take(5).ToList(),2)
            };
            var aggregate = new Order(events);

            var domainEvents = aggregate.Decide(new TakeMarchandise(colisList.Take(2).ToList()));

            Assert.Contains(domainEvents, e => e is MarchandiseReceived);
        }

        [Fact]
        public void Given_OrderStartedOf7Colis_With_3ColisReceived_When_TakeMarchandise_With_2Colis_Then_Raise_MarchandisePartiallyReceived()
        {
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
            var events = new List<IDomainEvent>
            {
                new OrderStarted(colisList,7),
                new MarchandisePartiallyReceived(colisList.Take(3).ToList(),4)
            };
            var aggregate = new Order(events);

            var domainEvents = aggregate.Decide(new TakeMarchandise(colisList.Take(2).ToList()));

            Assert.Contains(domainEvents, e => e is MarchandisePartiallyReceived);
        }
    }
}