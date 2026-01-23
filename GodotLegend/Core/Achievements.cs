using System;
using System.Collections.Generic;
using System.Text;

    public class PoleGameAchievement
    {
        public int AchievementGuid;
        public int AchievedCode;
        public string Name;
        public string Description;
        public int Value;
        public bool Secret;
        public bool Achieved = false;
        public DateTime WhenAchieved;


        public PoleGameAchievement(int achievementGuid, int achievedCode, string name, int value, bool secret, string description)
        {
            AchievementGuid = achievementGuid; AchievedCode = achievedCode;
            Name = name; Description = description; Value = value; Secret = secret;
        }

        public static List<PoleGameAchievement> List = new List<PoleGameAchievement>();
        static PoleGameAchievement() 
        {
            // Added
            List.Add(new PoleGameAchievement(2002, 3242, "Show Some Discipline", 10, false, "Proudly display a Discipline bar on your Engineering jacket."));
            // Added
            List.Add(new PoleGameAchievement(543, 5324, "Desperate Times, Desperate Measures", 10, false, "Wind up your arm fully, completely before you toss a road apple."));
            // Added
            List.Add(new PoleGameAchievement(98, 12354, "It's The Jam! It's All Good For You", 20, false, "Offer 'za or a drink to the Engineering Society President."));
            // Added
            //List.Add(new PoleGameAchievement(10, 13213, "Give 'Em A Hand", 20, false, "Have Al 'Pop Boy' Burchell called in to help the frosh."));
            // Added
            List.Add(new PoleGameAchievement(69, 6542, "You're a Hoser", 20, false, "Cool down the frosh with water from the firehose."));
            // Added
            List.Add(new PoleGameAchievement(2000, 562, "Like Homecoming, but with Lanolin", 50, false, "Quench the crowd's thirst 'til they slam their leather jackets."));
            // Added
            List.Add(new PoleGameAchievement(5, 52, "Pole in Ten (Years!)", 100, false, "Stall the frosh for at least 10 minutes."));
            // Added
            //List.Add(new PoleGameAchievement(4, 2343, "Room for Everyone in There", 50, false, "Push an artsci or commie into the pit."));

            // Added
            List.Add(new PoleGameAchievement(1999, 373, "Golden Soda", 50, false, "Offer a drink to hard-working Al 'Pop Boy' Burchell."));
            // Added
            List.Add(new PoleGameAchievement(747, 474, "Dizzying Heights", 50, false, "Send a frosh flying from tam to pit-water."));
            // Added
            //List.Add(new PoleGameAchievement(404, "Well Rounded", 50, false, "Wield the four frosh-stalling implements simultaneously."));
            // Added
            List.Add(new PoleGameAchievement(13, 90210, "Exam Avoidance", 50, false, "Stall the frosh for five minutes without lobbing a physics 'smart bomb'."));
            // Added
            List.Add(new PoleGameAchievement(399, 9999991, "Fully Loaded Fun Fur", 70, false, "Stuff your pockets with 99 apples, 99 slices of 'za, or 99 Clark mugs."));
            // Added
            List.Add(new PoleGameAchievement(7, 62463, "Iron Ring Ceremony", 100, false, "Unleash the power of the mighty Iron Ring."));
            // Added
            List.Add(new PoleGameAchievement(7777, 32592, "Double Fisting", 150, false, "Wear two Iron Rings at the same time!"));

            // Added
            List.Add(new PoleGameAchievement(96, 13312, "Hungry Hungry Hippo", 50, true, "Feed anything to the Golden Words Hippo. He's not fussy."));
            // Added
            List.Add(new PoleGameAchievement(99, 5676, "How do you like THEM Apples?", 100, true, "Toss an apple at an artsci or commie in the pit."));
            // Added
            List.Add(new PoleGameAchievement(11, 24601, "You messed up!", 20, true, "Receive a Tri-Pub Ban - a dubious distiction at best!"));
            // Added
            List.Add(new PoleGameAchievement(50, 3423, "Work Hard, Party Harder", 50, true, "Quaff some golden soda. It'd be rude not to."));
            // Added
            //List.Add(new PoleGameAchievement(12, 343, "Pole in Twenty", 150, true, "Stall the frosh for at least 20 minutes."));
        }
    }
