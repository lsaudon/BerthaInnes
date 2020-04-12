using System.Collections.Generic;
using BerthaInnes.Domain.CommandSide.DomainEvents;
using BerthaInnes.Domain.QuerySide;
using BerthaInnes.Infrastructure.EventStore;
using Xunit;

namespace BerthaInnes.Tests.Infrastructure.EventStore
{
    public class EventStoreInEventStoreTests
    {
        [Fact]
        public void
            Should_Return_All_Events_When_Get_All_Events_Of_Aggregate_Instance_After_Store_Events_Of_An_Aggregate_Instance()
        {
        }
    }
}