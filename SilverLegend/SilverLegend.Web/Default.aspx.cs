using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SilverLegend.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var lbs = new LeaderboardSvc();
            var analytics = new AnalyticsSvc();
            int longGame = lbs.GetLongestGame();
            int achHighest = lbs.GetHighestAchievementTotal();

            int gamesPlayed = lbs.GetGamesStartedCount();
            int achTotal = analytics.GetTotalAchievementCount();

            TimeSpan t = FromHighScoreInt(longGame);

            this.HighScoresToolTip.Text =
                "Worldwide best time ever: "
                 + ToHighScoreString(t) +
                " Best achievement total: " +
                achHighest;
            this.HighScoresToolTip.ToolTip = "Think you can beat these?\r\n (" + gamesPlayed.ToString()
            + " Games Played, " + achTotal.ToString() + " achievements total)";
        }

        public static string ToHighScoreString(TimeSpan t)
        {
            return t.Minutes.ToString("0") + ":" + t.Seconds.ToString("00") + "." + (t.Milliseconds / 10).ToString("00");
        }

        public static TimeSpan FromHighScoreInt(int highScore)
        {
            //int highScoreMinutes = (int)(highScore) / 1000 / 60;
            //int highScoreSeconds = (int)(highScore) / 1000 % 60;
            //int highScoreMilliseconds = (int)(highScore / 10) % 100;

            return TimeSpan.FromMilliseconds(highScore);

        }

    }
}