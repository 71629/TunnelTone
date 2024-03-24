using TunnelTone.UI;
using TunnelTone.UI.SongList;
using TunnelTone.UI.Transition;

namespace TunnelTone.Core
{
    public abstract class MusicPlayMode
    {
        public abstract void Quit();
    }

    public class FreePlay : MusicPlayMode
    {
        public override void Quit()
        {
            SongListManager.LoadSongList(new FreePlay());
        }
    }

    public class ProgressionPlay : MusicPlayMode
    {
        public override void Quit()
        {
            // Replace with to Story mode
            SongListManager.LoadSongList(new ProgressionPlay());
        }
    }
}