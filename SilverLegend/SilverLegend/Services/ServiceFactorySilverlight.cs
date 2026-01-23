using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverLegend.Services
{
    public class ServiceFactorySilverlight : ServiceFactory
    {
        public override IGameSettingsService ProduceGameSettingsService()
        {
            return new GameSettingsServiceSilverlight();
        }

        public override IGameTimerService ProduceGameTimerService()
        {
            return new GameTimerServiceSilverlight();
        }

        public override IInputService ProduceInputService()
        {
            return new InputServiceSilverlight();
        }
        public override IRenderingService ProduceRenderingService()
        {
            return new RenderingServiceSilverlight();
        }
        public override ISoundService ProduceSoundService()
        {
            return new SoundServiceSilverlight();
        }
    }
}
