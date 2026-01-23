using System;
using System.Collections.Generic;
using System.Text;

public class ServiceFactory
{
    public static ServiceFactory Instance;


    public virtual IGameSettingsService ProduceGameSettingsService()
    {
        return null;
    }
    public virtual IInputService ProduceInputService()
    {
        return null;
    }
    public virtual IRenderingService ProduceRenderingService()
    {
        return null;
    }
    public virtual ISoundService ProduceSoundService()
    {
        return null;
    }
    public virtual IGameTimerService ProduceGameTimerService()
    {
        return null;
    }
}
