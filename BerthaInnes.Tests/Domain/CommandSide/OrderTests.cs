using System.Collections.Generic;
using System.Linq;
using BerthaInnes.Domain.CommandSide;
using BerthaInnes.Domain.CommandSide.DomainCommands;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using Xunit;

namespace BerthaInnes.Tests.Domain.CommandSide
{
    public class OrderTests
    {
        [Fact]
        public void When_StartOrder_Then_Raise_OrderStarted()
        {
            var domainEvents = Order.Decide(new StartOrder(new List<Colis>()), new List<IDomainEvent>());

            Assert.Contains(domainEvents, e => e is OrderStarted);
        }

        [Fact]
        public void Given_OrderStarted_When_OrderStarted_Then_Raise_Nothing()
        {
            var events = new List<IDomainEvent> { new OrderStarted(0) };

            var domainEvents = Order.Decide(new StartOrder(new List<Colis>()), events);
            Assert.Empty(domainEvents);
        }

        [Fact]
        public void Given_OrderStarted_When_send_TakeMarchandise_Then_Raise_MarchandiseReceived()
        {
            var events = new List<IDomainEvent> { new OrderStarted(0) };

            var domainEvents = Order.Decide(new TakeMarchandise(new List<Colis>()), events);

            Assert.Contains(domainEvents, e => e is MarchandiseReceived);
        }

        [Fact]
        public void Given_OrderWithMarchandiseReceived_When_TakeMarchandise_Then_Raise_Nothing()
        {
            var events = new List<IDomainEvent> { new OrderStarted(0), new MarchandiseReceived() };

            var domainEvents = Order.Decide(new TakeMarchandise(new List<Colis>()), events);
            Assert.Empty(domainEvents);
        }

        [Fact]
        public void When_TakeMarchandise_Then_Raise_Nothing()
        {
            var domainEvents = Order.Decide(new TakeMarchandise(new List<Colis>()), new List<IDomainEvent>());
            Assert.Empty(domainEvents);
        }

        [Fact]
        public void Given_OrderStartedOf7Colis_When_TakeMarchandiseWith5Colis_Then_Raise_MarchandisePartiallyReceived()
        {
            var events = new List<IDomainEvent> { new OrderStarted(7) };

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
            var domainEvents = Order.Decide(new TakeMarchandise(colisList.Take(5).ToList()), events);

            Assert.Contains(domainEvents, e => e is MarchandisePartiallyReceived);
        }

        [Fact]
        public void Given_OrderOf7Colis_With_5ColisReceived_When_TakeMarchandiseWith2Colis_Then_Raise_MarchandiseReceived()
        {
            var events = new List<IDomainEvent>
            {
                new OrderStarted(7),
                new MarchandisePartiallyReceived(2)
            };

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
            var domainEvents = Order.Decide(new TakeMarchandise(colisList.Take(2).ToList()), events);

            Assert.Contains(domainEvents, e => e is MarchandiseReceived);
        }

        [Fact]
        public void Given_OrderStartedOf7Colis_With_3ColisReceived_When_TakeMarchandise_With_2Colis_Then_Raise_MarchandisePartiallyReceived()
        {
            var events = new List<IDomainEvent>
            {
                new OrderStarted(7),
                new MarchandisePartiallyReceived(4)
            };

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
            var domainEvents = Order.Decide(new TakeMarchandise(colisList.Take(2).ToList()), events).ToList();
            Assert.Single(domainEvents);
            Assert.Equal(new MarchandisePartiallyReceived(2), (MarchandisePartiallyReceived)domainEvents.First());
        }

        [Fact]
        public void Given_OrderStartedOf2Colis_When_TakeMarchandiseWith3Colis_Then_Raise_MarchandiseReceivedWith1Colis_MarchandiseExcessReceivedWith1Colis()
        {
            var events = new List<IDomainEvent>
            {
                new OrderStarted(2),
            };

            var colisList = new List<Colis>
            {
                new Colis(),
                new Colis(),
                new Colis(),
            };
            var domainEvents = Order.Decide(new TakeMarchandise(colisList.ToList()), events).ToList();
            Assert.Equal(2, domainEvents.Count);
            Assert.Contains(new MarchandiseReceived(2), domainEvents);
            Assert.Contains(new MarchandiseExcessReceived(1), domainEvents);
        }
    }
}