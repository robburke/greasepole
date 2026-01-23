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
    /// <summary>
    /// COMPLETE but untested: Game timer service using .NET's DateTime.Now.
    /// Observation: This Service is unnecessary as the implementation will be the same across different 
    /// versions of Greasepole.  Instead the single private CurrentTime() function is really the only thing needed
    /// to be abstracted out.
    /// </summary>
    public class GameTimerServiceSilverlight : IGameTimerService
    {
        private bool gameTimerActive = true;
        private double totalGameTime = 0;
        private double basePause = 0;
        private double lastUpdateTime = -1;
        private double accumulatedTimeTowardUpdates = 0;

        private const double TimePerUpdate = 1000.0 / 24.0;
//        private const double TimePerUpdate = 1000.0 / 5.0;

        public void Update()
        {
            double currentTime = CurrentTime();
            if (lastUpdateTime == -1)
            {
                lastUpdateTime = currentTime;
            }
            double deltaTime = currentTime - lastUpdateTime;
            if (gameTimerActive)
            {
                totalGameTime += deltaTime;
                accumulatedTimeTowardUpdates += deltaTime;
            }
            lastUpdateTime = currentTime;
        }

        public int GetAdditionalUpdateCount()
        {
            return 1; // NOTE: For now, in Silverlight version, we tether this.
            //return 24 / Util.GameLoop.RenderFps;
            //int additionalUpdates = 0;
            //while (accumulatedTimeTowardUpdates > TimePerUpdate)
            //{
            //    additionalUpdates++;
            //    accumulatedTimeTowardUpdates -= TimePerUpdate;
            //}
            //return additionalUpdates;
        }

        public void PauseUpdateCountTimer()
        {
            if (gameTimerActive)
            {
                gameTimerActive = false;
                basePause = CurrentTime();
            }
        }

        public void ResumeUpdateCountTimer()
        {
            if (!gameTimerActive)
            {
                gameTimerActive = true;
                double lostTime = CurrentTime() - basePause;
                accumulatedTimeTowardUpdates -= lostTime;
            }
        }

        private DateTime BaseDateTime = DateTime.Now;
        private double CurrentTime()
        {
            DateTime now = DateTime.Now;

            return ((now - BaseDateTime)).TotalMilliseconds;
        }

        public void ResetGameTimeScore()
        {
            totalGameTime = 0;
        }

        public double GetCurrentGameTimeScoreMilliseconds()
        {
            return totalGameTime;
        }

    }
}
