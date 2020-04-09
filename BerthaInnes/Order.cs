using System.Collections.Generic;
using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;

namespace BerthaInnes
{
    public class Order
    {
        private readonly DecisionProjection _decisionProjection;

        public Order(IEnumerable<DomainEvent> domainEvents)
        {
            _decisionProjection = new DecisionProjection();
            HydrateProjection(domainEvents);
        }

        public List<DomainEvent> Decide(DomainCommand command)
        {
            var domainEvents = command switch
            {
                StartOrder startOrder => Decide(startOrder),
                TakeMarchandise takeMarchandise => Decide(takeMarchandise),
                _ => new List<DomainEvent>()
            };

            HydrateProjection(domainEvents);

            return domainEvents;
        }

        private List<DomainEvent> Decide(StartOrder startOrder)
        {
            return new List<DomainEvent> { new OrderStarted(startOrder.ColisList, startOrder.ColisList.Count) };
        }

        private List<DomainEvent> Decide(TakeMarchandise takeMarchandise)
        {
            if (_decisionProjection.IsMarchandiseReceived) return new List<DomainEvent>();

            if (!_decisionProjection.IsStarted) return new List<DomainEvent>();

            if (_decisionProjection.NumberColisRemaining > takeMarchandise.ColisList.Count)
            {
                var numberColisRemaining = _decisionProjection.NumberColisRemaining - takeMarchandise.ColisList.Count;
                return new List<DomainEvent>
                {
                    new MarchandisePartiallyReceived(takeMarchandise.ColisList, numberColisRemaining)
                };
            }

            return new List<DomainEvent> { new MarchandiseReceived() };
        }

        private void HydrateProjection(IEnumerable<DomainEvent> domainEvents)
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

            public void Apply(DomainEvent domainEvent)
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
