using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SilverLegend.Web
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Random r = new Random();
            int longGameNew = r.Next(1, 10);
            int achTotNew = r.Next(11, 20);

            //new LeaderboardSvc().UpdateLeaderboard(longGameNew, achTotNew);

            int longGame = new LeaderboardSvc().GetLongestGame();
            int achTot = new LeaderboardSvc().GetHighestAchievementTotal();
            int[] topScores = new LeaderboardSvc().GetTopTenLongestGames();
            this.longestGame.Text = FromHighScoreInt(longGame).ToString();
            this.maxTotalAchievement.Text = achTot.ToString();
            string allScores = "";
            foreach (int i in topScores)
            {
                allScores += FromHighScoreInt(i).ToString() + " ";
            }
            this.topTenScoresLabel.Text = allScores;
        }

        public static TimeSpan FromHighScoreInt(int hsInt)
        {
            int seconds = (hsInt / 10) % 100;
            int minutes = hsInt / 1000 % 60;
            int hours = hsInt / 1000 / 60;

            return new TimeSpan(hours, minutes, seconds);

        }

    }
}