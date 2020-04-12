using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public class PendingOrderEventHandler : IEventHandler<OrderStarted>, IEventHandler<MarchandiseReceived>
    {
        private readonly List<WaitingOrder> _repository;

        public PendingOrderEventHandler(List<WaitingOrder> repository)
        {
            _repository = repository;
        }

        public void Handle(OrderStarted evt)
        {
            var waitingOrder = new WaitingOrder(evt.Id, evt.NumberColis);
            _repository.Add(waitingOrder);
        }

        public void Handle(MarchandiseReceived evt)
        {
            _repository.RemoveAll(w => Equals(w.Id, evt.Id));
        }

        public void Handle(EventsWrapper evt)
        {
            foreach (var domainEvent in evt.DomainEvents)
            {
                switch (domainEvent)
                {
                    case OrderStarted orderStarted:
                    {
                        var waitingOrder = new WaitingOrder(evt.AggregateId, orderStarted.NumberColis);
                        _repository.Add(waitingOrder);
                        break;
                    }
                    case MarchandiseReceived _:
                    {
                        _repository.RemoveAll(w => Equals(w.Id, evt.AggregateId));
                        break;
                    }
                }
            }
        }
    }
}