namespace TunnelTone.Core
{
    public interface IOnPauseHandler
    {
        void OnPause();
        void OnResume();
        void OnRetry();
        void OnQuit();
    }
}