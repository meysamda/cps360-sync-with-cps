using Xunit;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Common
{
    [CollectionDefinition("Collection-1")]
    public class MessageBusCollectionFixture : ICollectionFixture<MessageBusFixture>
    {
    }
}