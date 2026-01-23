using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using SilverLegend.LeaderboardSvcRef;

namespace SilverLegend.Services
{
    /// <summary>
    /// COMPLETE: Service which saves settings to Silverlight IsolatedStorage
    /// </summary>
    public class GameSettingsServiceSilverlight : IGameSettingsService
    {

        public void SaveSettings(List<PoleGameSetting> settings)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream =
                    new IsolatedStorageFileStream("Legend2007Silverlight.sav",
                        FileMode.Create, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        foreach (PoleGameSetting pgs in settings)
                        {
                            writer.WriteLine(pgs.Name + "|" + pgs.Value.ToString());
                        }
                    }
                }
            }


            int achievementValueTotal = 0;
            foreach (PoleGameAchievement pga in PoleGameAchievement.List)
            {
                if (pga.Achieved) achievementValueTotal += pga.Value;
            }
            int highScore = Globals.myGameConditions.GetHighScore(0);
            if (achievementValueTotal != LastAchievementScoreSent || highScore != LastHighScoreTimeSent)
            {
                LastHighScoreTimeSent = highScore;
                LastAchievementScoreSent = achievementValueTotal;
                if (!(highScore == 0 && achievementValueTotal == 0))
                {
                    LeaderboardSvcClient soapClient = new LeaderboardSvcClient();
                    soapClient.UpdateLeaderboardAsync(highScore, achievementValueTotal);
                }
            }
        }

        public int LastAchievementScoreSent = -1;
        public int LastHighScoreTimeSent = -1;


        public void ResetSettings()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    isoStore.DeleteFile("Legend2007Silverlight.sav");
                }
                catch (Exception) { }
            }
        }

        public List<PoleGameSetting> LoadSettings()
        {
            List<PoleGameSetting> settings = new List<PoleGameSetting>();
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (IsolatedStorageFileStream isoStream =
                        new IsolatedStorageFileStream("Legend2007Silverlight.sav",
                            FileMode.Open, isoStore))
                    {
                        using (StreamReader reader = new StreamReader(isoStream))
                        {
                            while (!reader.EndOfStream)
                            {
                                String sb = reader.ReadLine();
                                string[] substrings = sb.Split('|');
                                if (substrings.Length != 2) continue;
                                string name = substrings[0];
                                int value = 0;
                                try
                                {
                                    value = Int32.Parse(substrings[1]);
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                                settings.Add(new PoleGameSetting(name, value));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // fail.  Not an issue though, there are default values for all settings.
                }
            }
            return settings;
        }

    }
}

