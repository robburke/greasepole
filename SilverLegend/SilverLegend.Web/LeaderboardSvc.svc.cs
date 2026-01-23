using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;

namespace SilverLegend.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LeaderboardSvc
    {
        [OperationContract]
        public void UpdateLeaderboard(int longestGame, int achievementTotal)
        {
            string ipAddress = AnalyticsUtil.GetIpAddressOfCurrentOperationContext();

            using (GreasepoleEntities ge = new GreasepoleEntities())
            {
                Notification hsNotification = new Notification()
                {
                    LongestGame = longestGame,
                    AchievementTotal = achievementTotal,
                    NotificationTime = DateTime.Now,
                    IPAddress = ipAddress
                };

                ge.AddToNotifications(hsNotification);
                ge.SaveChanges();
            }
            return;

        }

        [OperationContract]
        public int GetLongestGame()
        {
            using (GreasepoleEntities ge = new GreasepoleEntities())
            {
                var leaderboard = (from d in ge.Notifications
                                   orderby d.LongestGame descending
                                   select d.LongestGame).Take(1);

                if (leaderboard.Count() > 0)
                {
                    return leaderboard.First();
                }
            }

            return 0;
        }

        [OperationContract]
        public int GetHighestAchievementTotal()
        {
            using (GreasepoleEntities ge = new GreasepoleEntities())
            {
                var leaderboard = (from d in ge.Notifications
                                   orderby d.AchievementTotal descending
                                   select d.AchievementTotal).Take(1);

                if (leaderboard.Count() > 0)
                {
                    return leaderboard.First();
                }
            }
            return 0;
        }

        [OperationContract]
        public int[] GetTopTenLongestGames()
        {
            int numberToGet = 10;
            int[] topTen = new int[numberToGet];

            using (GreasepoleEntities ge = new GreasepoleEntities())
            {
                var leaderboard = ((from d in ge.Notifications
                                    orderby d.LongestGame
                                    select d.LongestGame).Distinct());

                int current = 0;
                foreach (var result in leaderboard)
                {
                    topTen[current] = result;
                    current++;
                    if (current == numberToGet) break;
                }
            }

            return topTen;
        }

        [OperationContract]
        public int GetGamesStartedCount()
        {
            using (var ae = new AnalyticsEntities())
            {
                var gamesPlayedCount = (from a in ae.Analytics
                                        where
                                        (a.Notification == "GameStateChange" && a.Parameters == "STATEGAME")
                                        select a.Id).Count();
                return gamesPlayedCount;
                                       
            }
        }


    }
}
