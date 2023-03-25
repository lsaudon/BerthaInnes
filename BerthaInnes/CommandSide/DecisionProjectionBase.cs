using System;
using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;

namespace BerthaInnes.Domain.CommandSide
{
    public abstract class DecisionProjectionBase
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> _handlersByType = new();

        public void Apply(IDomainEvent evt)
        {
            if (_handlersByType.TryGetValue(evt.GetType(), out var apply))
            {
                apply(evt);
            }
        }

        protected void AddHandler<T>(Action<T> apply)
            where T : IDomainEvent
        {
            _handlersByType.Add(typeof(T), o => apply((T)o));
        }
    }
}
