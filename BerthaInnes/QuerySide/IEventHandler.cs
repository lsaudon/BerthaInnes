using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.QuerySide
{
    public interface IEventHandler
    {
        void Handle(IDomainEvent evt);
    }
}