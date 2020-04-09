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
            var domainEvents = Order.Decide(new StartOrder(new List<Colis>()), new List<IDomainEvent>());

            Assert.Contains(domainEvents, e => e is OrderStarted);
        }

        [Fact]
        public void Given_OrderStarted_When_send_TakeMarchandise_Then_Raise_MarchandiseReceived()
        {
            var events = new List<IDomainEvent> { new OrderStarted(0) };

            var domainEvents = Order.Decide(new TakeMarchandise(new List<Colis>()), events);

            Assert.Contains(domainEvents, e => e is MarchandiseReceived);
        }

        [Fact]
        public void Given_OrderStarted_When_OrderStarted_Then_Raise_Nothing()
        {
            var events = new List<IDomainEvent> { new OrderStarted(0) };

            var domainEvents = Order.Decide(new StartOrder(new List<Colis>()), events);
            Assert.Empty(domainEvents);
        }

        [Fact]
        public void When_Send_TakeMarchandise_Then_Raise_MarchandiseReceived()
        {
            var domainEvents = Order.Decide(new TakeMarchandise(new List<Colis>()), new List<IDomainEvent>());

            Assert.Empty(domainEvents);
        }

        [Fact]
        public void Given_OrderWithMarchandiseReceived_When_TakeMarchandise_Then_Raise_Nothing()
        {
            var events = new List<IDomainEvent> { new OrderStarted(0), new MarchandiseReceived() };

            var domainEvents = Order.Decide(new TakeMarchandise(new List<Colis>()), events);
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
            var events = new List<IDomainEvent> { new OrderStarted(7) };

            var domainEvents = Order.Decide(new TakeMarchandise(colisList.Take(5).ToList()), events);

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
                new OrderStarted(7),
                new MarchandisePartiallyReceived(2)
            };

            var domainEvents = Order.Decide(new TakeMarchandise(colisList.Take(2).ToList()), events);

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
                new OrderStarted(7),
                new MarchandisePartiallyReceived(4)
            };

            var domainEvents = Order.Decide(new TakeMarchandise(colisList.Take(2).ToList()), events);

            Assert.Contains(domainEvents, e => e is MarchandisePartiallyReceived);
        }
    }
}