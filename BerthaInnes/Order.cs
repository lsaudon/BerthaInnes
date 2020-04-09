using System.Collections.Generic;
using System.Linq;
using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;

namespace BerthaInnes
{
    public class Order
    {
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();

        public List<DomainEvent> Decide(DomainCommand command)
        {
            return command switch
            {
                StartOrder _ => DecideForStartOrder(),
                TakeMarchandise _ => DecideForTakeMarchandise(),
                _ => new List<DomainEvent>()
            };
        }

        private List<DomainEvent> DecideForStartOrder()
        {
            var orderStarted = new OrderStarted();
            _domainEvents.Add(orderStarted);
            return new List<DomainEvent> { orderStarted };
        }

        private List<DomainEvent> DecideForTakeMarchandise()
        {
            if (_domainEvents.Any(domainEvent => domainEvent is MarchandiseReceived))
            {
                return new List<DomainEvent>();
            }

            if (_domainEvents.All(domainEvent => domainEvent.GetType() != typeof(OrderStarted)))
            {
                return new List<DomainEvent>();
            }

            var marchandiseReceived = new MarchandiseReceived();
            _domainEvents.Add(marchandiseReceived);
            return new List<DomainEvent> { marchandiseReceived };
        }
    }
}
