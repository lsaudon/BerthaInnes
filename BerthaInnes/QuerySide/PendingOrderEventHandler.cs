using System.Collections.Generic;
using BerthaInnes.CommandSide.DomainEvents;

namespace BerthaInnes.QuerySide
{
    public class PendingOrderEventHandler
    {
        private readonly List<WaitingOrder> _repository;

        public PendingOrderEventHandler(List<WaitingOrder> repository)
        {
            _repository = repository;
        }

        public void Handle(EventWrapper evt)
        {
            switch (evt.DomainEvent)
            {
                case OrderStarted _:
                    {
                        var waitingOrder = new WaitingOrder(evt.AggregateId);
                        _repository.Add(waitingOrder);
                        break;
                    }
                case MarchandiseReceived _:
                    {
                        _repository.RemoveAll(w => w.Id == evt.AggregateId);
                        break;
                    }
            }
        }
    }
}