using BerthaInnes.Infrastructure;
using Xunit;

namespace BerthaInnes.Tests
{
    public class EventStoreTests
    {
        [Fact]
        public void
            Should_Return_All_Events_When_Get_All_Events_Of_Aggregate_Instance_After_Store_Events_Of_An_Aggregate_Instance()
        {
            new EventStore();
        }
    }
}