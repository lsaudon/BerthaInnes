using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public class PendingOrderEventHandler : IEventHandler
    {
        private readonly List<WaitingOrder> _repository;

        public PendingOrderEventHandler(List<WaitingOrder> repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent evt)
        {
            if (evt is OrderStarted orderStarted)
            {
                var waitingOrder = new WaitingOrder(orderStarted.Id, orderStarted.NumberColis);
                _repository.Add(waitingOrder);
            }
            else if (evt is MarchandiseReceived marchandiseReceived)
            {
                _repository.RemoveAll(w => Equals(w.Id, marchandiseReceived.Id));
            }
        }
    }
}