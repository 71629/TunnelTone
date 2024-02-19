using System;
using TunnelTone.UI.Dialog;

namespace TunnelTone.Exceptions
{
    public class TunnelToneException : Exception
    {
        protected TunnelToneException(string message) : base(message)
        {
            
        }
    }
    
    public class ChartNotFoundException : TunnelToneException
    {
        public ChartNotFoundException(string message) : base(message)
        {
            
        }
    }
    
    public class StoryNotLoadedException : TunnelToneException
    {
        public StoryNotLoadedException(string message) : base(message)
        {
            
        }
    }
    
    public class CorruptedPlayerSettingsException : TunnelToneException
    {
        public CorruptedPlayerSettingsException(string message) : base(message)
        {
            
        }
    }

    public class CorruptedPlayerProfileException : TunnelToneException
    {
        public CorruptedPlayerProfileException(string message) : base(message)
        {

        }
    }
}