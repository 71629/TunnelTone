using System;

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
        public ChartNotFoundException(string message = null) : base(message)
        {
            
        }
    }
    
    public class StoryNotLoadedException : TunnelToneException
    {
        public StoryNotLoadedException(string message = null) : base(message)
        {
            
        }
    }
    
    public class CorruptedPlayerSettingsException : TunnelToneException
    {
        public CorruptedPlayerSettingsException(string message = null) : base(message)
        {
            
        }
    }

    public class CorruptedPlayerProfileException : TunnelToneException
    {
        public CorruptedPlayerProfileException(string message = null) : base(message)
        {

        }
    }

    public class StoryIsNullException : TunnelToneException
    {
        public StoryIsNullException(string message = null) : base(message)
        {

        }
    }
}