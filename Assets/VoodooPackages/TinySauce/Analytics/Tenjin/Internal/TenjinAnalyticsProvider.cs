
namespace Voodoo.Sauce.Internal.Analytics
{
    internal class TenjinAnalyticsProvider : IAnalyticsProvider
    {
        public void Initialize( bool consent)
        {
            TenjinWrapper.Initialize(consent);
            RegisterEvents();
        }

        private static void RegisterEvents()
        {
            AnalyticsManager.OnApplicationResumeEvent += OnApplicationResume;
        }
        private static void OnApplicationResume()
        {
            TenjinWrapper.Connect();
        }
    }
}