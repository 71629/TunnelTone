using TunnelTone.Singleton;

namespace TunnelTone.Events
{
    public class SystemEventReference : Singleton<SystemEventReference>
    {
        public readonly GameEvent OnChartLoad = new GameEvent();
        public readonly GameEvent OnChartLoadFinish = new GameEvent();

        public readonly GameEvent OnDisplayDialog = new GameEvent();
    }
}