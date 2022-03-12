namespace Cps360.SyncWithCps.Presentation.Common
{
    public interface ICpsPortfolioPublishMessageBus
    {
        void Publish(CpsPortfolioMessage message);
    }
}