using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SilverLegend.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AnalyticsSvc
    {
        [OperationContract]
        public void Yo(int gameId, string notification, string parameters)
        {
            string ipAddress = AnalyticsUtil.GetIpAddressOfCurrentOperationContext();

            using (AnalyticsEntities ae = new AnalyticsEntities())
            {
                Analytic a = new Analytic()
                {
                    IPAddress = ipAddress,
                    Time = DateTime.Now,
                    GameId = gameId,
                    Notification = notification,
                    Parameters = parameters
                };
                ae.AddToAnalytics(a);
                ae.SaveChanges();
            }
            return;
        }

        // Add more operations here and mark them with [OperationContract]

        public int GetTotalAchievementCount()
        {
            int count = 0;
            using (var ae = new AnalyticsEntities())
            {
                count =
                    (from d in ae.Analytics
                     where (d.Notification == "GameAnalytic" && d.Parameters.StartsWith("AchUnl;"))
                     select d.Id).Count();
            }
            return count;
        }
    }
}
