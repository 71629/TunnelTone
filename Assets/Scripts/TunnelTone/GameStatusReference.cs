using TunnelTone.Singleton;
using TunnelTone.Enumerators;

namespace TunnelTone
{
    public class GameStatusReference : Singleton<GameStatusReference>
    {
        public PlayerStatus PlayerStatus { get; set; }
        public GameStatus GameStatus { get; set; }
    }
}