using System.Collections.Generic;
using BerthaInnes.CommandSide.DomainCommands;
using BerthaInnes.CommandSide.DomainEvents;

namespace BerthaInnes.CommandSide
{
    public class Order
    {
        public static IEnumerable<IDomainEvent> Decide(IDomainCommand command, IEnumerable<IDomainEvent> pastEvents)
        {
            var decisionProjection = new DecisionProjection();
            foreach (var domainEvent in pastEvents)
            {
                decisionProjection.Apply(domainEvent);
            }

            var uncommittedEvents = command switch
            {
                StartOrder startOrder => Decide(startOrder, decisionProjection),
                TakeMarchandise takeMarchandise => Decide(takeMarchandise, decisionProjection),
                _ => new List<IDomainEvent>()
            };

            return uncommittedEvents;
        }

        private static IEnumerable<IDomainEvent> Decide(StartOrder startOrder, DecisionProjection decisionProjection)
        {
            if (decisionProjection.IsStarted) return new List<IDomainEvent>();

            return new List<IDomainEvent> { new OrderStarted(startOrder.ColisList.Count) };
        }

        private static IEnumerable<IDomainEvent> Decide(TakeMarchandise takeMarchandise, DecisionProjection decisionProjection)
        {
            if (decisionProjection.IsMarchandiseReceived) return new List<IDomainEvent>();

            if (!decisionProjection.IsStarted) return new List<IDomainEvent>();

            if (decisionProjection.NumberColisRemaining > takeMarchandise.ColisList.Count)
            {
                var numberColisRemaining = decisionProjection.NumberColisRemaining - takeMarchandise.ColisList.Count;
                return new List<IDomainEvent>
                {
                    new MarchandisePartiallyReceived(numberColisRemaining)
                };
            }

            if (decisionProjection.NumberColisRemaining < takeMarchandise.ColisList.Count)
            {
                var numberColisExcess = takeMarchandise.ColisList.Count - decisionProjection.NumberColisRemaining;
                return new List<IDomainEvent>
                {
                    new MarchandiseReceived(decisionProjection.NumberColisRemaining),
                    new MarchandiseExcessReceived(numberColisExcess)
                };
            }

            return new List<IDomainEvent> { new MarchandiseReceived(0) };
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
                NumberColisRemaining = marchandiseReceived.NumberColisRemaining;
            }
        }
    }
}
