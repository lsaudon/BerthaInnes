using System.Collections.Generic;
using System.Linq;
using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;

namespace BerthaInnes
{
    public class Order
    {
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();

        public DomainEvent Decide(DomainCommand command)
        {
            return command switch
            {
                StartOrder _ => DecideForStartOrder(),
                TakeMarchandise _ => DecideForTakeMarchandise(),
                _ => new Nothing()
            };
        }

        private DomainEvent DecideForStartOrder()
        {
            var orderStarted = new OrderStarted();
            _domainEvents.Add(orderStarted);
            return orderStarted;
        }

        private DomainEvent DecideForTakeMarchandise()
        {
            if (_domainEvents.Any(domainEvent => domainEvent.GetType() == typeof(MarchandiseReceived)))
            {
                return new Nothing();
            }

            if (_domainEvents.All(domainEvent => domainEvent.GetType() != typeof(OrderStarted)))
            {
                return new Nothing();
            }

            var marchandiseReceived = new MarchandiseReceived();
            _domainEvents.Add(marchandiseReceived);
            return marchandiseReceived;
        }
    }
}
