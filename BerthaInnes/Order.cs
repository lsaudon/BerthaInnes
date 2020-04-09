using System.Collections.Generic;
using BerthaInnes.DomainCommands;
using BerthaInnes.DomainEvents;

namespace BerthaInnes
{
    public class Order
    {
        private DecisionProjection _decisionProjection;

        public List<DomainEvent> Decide(DomainCommand command)
        {
            return command switch
            {
                StartOrder startOrder => DecideForStartOrder(startOrder),
                TakeMarchandise takeMarchandise => DecideForTakeMarchandise(takeMarchandise),
                _ => new List<DomainEvent>()
            };
        }

        private List<DomainEvent> DecideForStartOrder(StartOrder startOrder)
        {
            _decisionProjection.IsStarted = true;
            _decisionProjection.ColisNumber = startOrder.ColisList.Count;
            return new List<DomainEvent> { new OrderStarted() };
        }

        private List<DomainEvent> DecideForTakeMarchandise(TakeMarchandise takeMarchandise)
        {
            if (_decisionProjection.IsMarchandiseReceived)
            {
                return new List<DomainEvent>();
            }

            if (!_decisionProjection.IsStarted)
            {
                return new List<DomainEvent>();
            }

            if (_decisionProjection.ColisNumber > takeMarchandise.ColisList.Count)
            {
                _decisionProjection.ColisNumber -= takeMarchandise.ColisList.Count;
                return new List<DomainEvent> { new MarchandisePartiallyReceived() };
            }

            _decisionProjection.IsMarchandiseReceived = true;
            return new List<DomainEvent> { new MarchandiseReceived() };
        }

        private struct DecisionProjection
        {
            public bool IsStarted { get; set; }
            public bool IsMarchandiseReceived { get; set; }
            public int ColisNumber { get; set; }
        }
    }
}
