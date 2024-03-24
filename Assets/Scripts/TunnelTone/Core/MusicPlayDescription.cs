using TunnelTone.ScriptableObjects;
using UnityEngine;

namespace TunnelTone.Core
{
    public class MusicPlayDescription
    {
        public static MusicPlayDescription instance;
        
        public Sprite jacket;

        public SongData songData;
        public MusicPlayMode module;
        public int difficulty;
        public Chart chart;
        public AudioClip music;
        public PlayResult result;
        
        private MusicPlayDescription() { }

        public static void CreateInstance()
        {
            if (instance is not null)
                Debug.LogWarning("You're attempting to overwrite an existing MusicPlayDescription instance, this is not allowed.\nPlease run ResetInstance() before creating a new instance.");
            instance = new MusicPlayDescription();
        }

        // Remove after external usage
        // ReSharper disable once MemberCanBePrivate.Global
        public static void ResetInstance() => instance = null;
    }
}

namespace TunnelTone.Exceptions
{
    public class MusicPlayDescriptionException : TunnelToneException
    {
        protected MusicPlayDescriptionException(string message = null) : base(message) { }
    }

    public class JacketIsNullException : MusicPlayDescriptionException
    {
        public JacketIsNullException(string message = null) : base(message) { }
    }
}