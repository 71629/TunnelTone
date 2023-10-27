using TunnelTone.Enumerators;
using TunnelTone.Singleton;

namespace TunnelTone.GameSystem
{
    public class GameStatusReference : Singleton<GameStatusReference>
    {
        public PlayerStatus PlayerStatus { get; set; }
        public GameStatus GameStatus { get; set; }
    }
}