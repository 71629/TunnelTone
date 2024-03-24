using System;
using TunnelTone.UI.Dialog;

namespace TunnelTone.Exceptions
{
    public class TunnelToneException : Exception
    {
        private const string ErrorPrefix = "An error occurred, TunnelTone is now unstable.";
        private const string ErrorSuffix = "If you choose to exit, your ongoing progress will be lost.";
        
        protected TunnelToneException(string message) : base(message)
        {
            DisplayWarning(message);
        }

        private static void DisplayWarning(string message)
        {
            message = $"{ErrorPrefix}\n\n{message}\n\n{ErrorSuffix}";
            DialogOption[] options =
            {
                new("Exit", Exit),
                new("Ignore", Ignore)
            };
            
            Dialog.OnDisplayDialog.Invoke("Error", message, options);
            return;
            
            void Exit()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
                using var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                    .GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("finishAffinity");
#elif UNITY_IOS
                Application.Quit();
#endif
            }

            void Ignore()
            {
                Dialog.OnAbortDialog.Invoke();
            }
        }
    }
    
    public class ChartNotFoundException : TunnelToneException
    {
        public ChartNotFoundException(string message = "Chart file is unresolved") : base(message)
        {
            
        }
    }
    
    public class StoryNotLoadedException : TunnelToneException
    {
        public StoryNotLoadedException(string message = "Story is not loaded correctly.") : base(message)
        {
            
        }
    }
    
    public class CorruptedPlayerSettingsException : TunnelToneException
    {
        public CorruptedPlayerSettingsException(string message = "Unable to read settings") : base(message)
        {
            
        }
    }

    public class CorruptedPlayerProfileException : TunnelToneException
    {
        public CorruptedPlayerProfileException(string message = "Player profile is unreadable.") : base(message)
        {

        }
    }
    
    public class StoryIsNullException : TunnelToneException
    {
        public StoryIsNullException(string message = "Cannot resolve story object") : base(message)
        {

        }
    }

    public class UnusedException : TunnelToneException
    {
        public UnusedException(string message = "Unknown error") : base(message)
        {

        }
    }
}