namespace Cps360.SyncWithCps.Presentation.Adapters
{
    public interface ICpsPortfolioPublishMessageBus
    {
        void Publish(CpsPortfolioMessage message);
    }
}