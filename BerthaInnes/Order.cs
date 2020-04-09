using System.Collections.Generic;
using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;

namespace BerthaInnes
{
    public class Order
    {
        private readonly DecisionProjection _decisionProjection;

        public Order(IEnumerable<IDomainEvent> domainEvents)
        {
            _decisionProjection = new DecisionProjection();
            HydrateProjection(domainEvents);
        }

        public List<IDomainEvent> Decide(IDomainCommand command)
        {
            var domainEvents = command switch
            {
                StartOrder startOrder => Decide(startOrder),
                TakeMarchandise takeMarchandise => Decide(takeMarchandise),
                _ => new List<IDomainEvent>()
            };

            HydrateProjection(domainEvents);

            return domainEvents;
        }

        private List<IDomainEvent> Decide(StartOrder startOrder)
        {
            return new List<IDomainEvent> { new OrderStarted(startOrder.ColisList.Count) };
        }

        private List<IDomainEvent> Decide(TakeMarchandise takeMarchandise)
        {
            if (_decisionProjection.IsMarchandiseReceived) return new List<IDomainEvent>();

            if (!_decisionProjection.IsStarted) return new List<IDomainEvent>();

            if (_decisionProjection.NumberColisRemaining > takeMarchandise.ColisList.Count)
            {
                var numberColisRemaining = _decisionProjection.NumberColisRemaining - takeMarchandise.ColisList.Count;
                return new List<IDomainEvent>
                {
                    new MarchandisePartiallyReceived(numberColisRemaining)
                };
            }

            return new List<IDomainEvent> { new MarchandiseReceived() };
        }

        private void HydrateProjection(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                _decisionProjection.Apply(domainEvent);
            }
        }

        private class DecisionProjection
        {
            public bool IsStarted { get; private set; }
            public bool IsMarchandiseReceived { get; private set; }
            public int NumberColisRemaining { get; private set; }

            public void Apply(IDomainEvent domainEvent)
            {
                switch (domainEvent)
                {
                    case OrderStarted orderStarted:
                        Apply(orderStarted);
                        break;
                    case MarchandisePartiallyReceived marchandisePartiallyReceived:
                        Apply(marchandisePartiallyReceived);
                        break;
                    case MarchandiseReceived marchandiseReceived:
                        Apply(marchandiseReceived);
                        break;
                }
            }

            private void Apply(OrderStarted orderStarted)
            {
                IsStarted = true;
                NumberColisRemaining = orderStarted.NumberColis;
            }

            private void Apply(MarchandisePartiallyReceived marchandisePartiallyReceived)
            {
                NumberColisRemaining = marchandisePartiallyReceived.NumberColisRemaining;
            }

            private void Apply(MarchandiseReceived marchandiseReceived)
            {
                IsMarchandiseReceived = true;
            }
        }
    }
}
