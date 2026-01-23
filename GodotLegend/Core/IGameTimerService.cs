using System;
using System.Collections.Generic;
using System.Text;

public interface IGameTimerService
{
    /// <summary>
    /// Call at the start of the AI Processing.
    /// </summary>
    void Update();
    /// <summary>
    /// Call after the Update to see how many times you should update the AI
    /// </summary>
    /// <returns></returns>
    int GetAdditionalUpdateCount();
    void PauseUpdateCountTimer();
    void ResumeUpdateCountTimer();

    /// <summary>
    /// Sets game timer clock to zero.
    /// </summary>
    void ResetGameTimeScore();
    /// <summary>
    /// Gets the total number of milliseconds that have passed in this game.
    /// </summary>
    double GetCurrentGameTimeScoreMilliseconds();

}
