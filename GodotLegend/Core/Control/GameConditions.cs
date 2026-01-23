using System.Collections.Generic;

public class GameConditions
{


    public bool gbTriPubBan = false;

    private int nNoiseCount;
    private int nTopples;
    private int[] nBooster = new int[AIMethods.NUM_PERFORMACEBOOSTS];
    private int nBoosterClock;
    private int[] nHighScore = new int[2];
    private bool bPopBoyInPit;
    private bool bRitual;
    private int nApples;
    private int nPizza;
    private int nClark;
    private int nExam;
    private int nHose;
    private int nRing;
    private bool[] bRingSpot = new bool[AIMethods.NUM_RINGSPOTS];
    private bool[] bSpecialRingTrick = new bool[AIMethods.NUM_TRICKS];
    private bool bIsDemo;
    private bool bGameOver;

    private int nAchievementGroup;
    private int nBarGroup;
    private int[] nJBar = new int[AIMethods.NUM_JBARSPOTS];
    private int nFroshLameness;
    private int nSound;
    private int nEnhancedGraphics;
    private int nRMBFunction;
    private int nDeltaEnergy;
    private int[] nPrezHit = new int[5]; // Relies on ((int)attrProjectile.attrProjectileType).  Apple, Grase, Clark, Pizza, Exam.


    public int CountTopples() { return nTopples; }
    public void Topple() { nTopples++; }
    public bool IsGameOver() { return bGameOver; }
    public bool IsDemo() { return bIsDemo; }
    public void GameOver() { bGameOver = true; }

    public bool UseTrick(int nIndex)
    {
        if (bSpecialRingTrick[nIndex]) return false;
        else return bSpecialRingTrick[nIndex] = true;
    }
    public void ReleaseTrick(int nIndex) { bSpecialRingTrick[nIndex] = false; }
    public bool IsTrickUsed(int nIndex) { return (bSpecialRingTrick[nIndex]); }

    public void TakeRingSpot(int nIndex) { bRingSpot[nIndex] = true; }
    public void ReleaseRingSpot(int nIndex) { bRingSpot[nIndex] = false; }
    public bool IsRingSpotOpen(int nIndex) { return !(bRingSpot[nIndex]); }

    public int GetBooster(int nIndex) { return nBooster[nIndex]; }

    public int GetNoiseCount() { return nNoiseCount; }
    public void SetNoiseCount(int nnewCount) { nNoiseCount = nnewCount; }

    public void AddEnergy(int nMore) { nDeltaEnergy += nMore; }
    public int GetEnergy() { return nDeltaEnergy; ;}
    public void ResetEnergy() { nDeltaEnergy = 0; }
    public void LoseApple() { nApples--; if (nApples < 0) nApples = 0; }
    public void LoseClark() { nClark--; }
    public void LosePizza() { nPizza--; if (nPizza < 0) nPizza = 0; }
    public void LoseExam() { nExam--; }
    public void LoseHose() { nHose--; }
    public void LoseRing() { nRing = 0; }
    public void GetApples(int nMore) { nApples += nMore; if (nApples > 99) nApples = 99; if (nApples < 0) nApples = 0; }
    public void GetClarks(int nMore) { nClark += nMore; if (nClark > 99) nClark = 99; if (nClark < 0) nClark = 0; }
    public void GetPizzas(int nMore) { nPizza += nMore; if (nPizza > 99) nPizza = 99; if (nPizza < 0) nPizza = 0; }
    public void GetExams(int nMore) { nExam += nMore; if (nExam > 14) nExam = 14; if (nExam < 0) nExam = 0; }
    public void GetHose(int nMore) { nHose += nMore; }
    public void GetRing(int nMore) { nRing += nMore; }
    public void SetBarGroup(int nNewBarGroup) { nBarGroup = nNewBarGroup; }
    public void SetAchievementGroup(int nNewAchievementGroup) { nAchievementGroup = nNewAchievementGroup; }

    public bool IsPopBoyInPit() { return bPopBoyInPit; }
    public void PopBoyJumpsIn() { bPopBoyInPit = true; }
    public int GetPrezHit(int nWeapon) { return nPrezHit[nWeapon]; }
    public void AddPrezHit(int nWeapon) { nPrezHit[nWeapon]++; }
    public void ResetPrezHit(int nWeapon) { nPrezHit[nWeapon] = 0; }

    public void SetFroshLameness(int nNew) { nFroshLameness = nNew; }
    public int GetFroshLameness() { return nFroshLameness; }
    public void SetSound(int nNew) { nSound = nNew; }
    public int GetSound() { return nSound; }
    public void SetEnhancedGraphics(int nNew) { nEnhancedGraphics = nNew; }
    public int GetEnhancedGraphics() { return nEnhancedGraphics; }
    public void SetRMBFunction(int nNew) { nRMBFunction = nNew; }
    public int GetRMBFunction() { return nRMBFunction; }


    public int GetJBar(int nBarSpot) { return nJBar[nBarSpot]; }

    public bool IsRitual() { return bRitual; }

    public int GetPlayerApples() { return nApples; }
    public int GetPlayerClark() { return nClark; }
    public int GetPlayerPizza() { return nPizza; }
    public int GetPlayerExam() { return nExam; }
    public int GetPlayerHose() { return nHose; }
    public int GetPlayerRing() { return nRing; }
    public int GetBarGroup() { return nBarGroup; }
    public int GetAchievementGroup() { return nAchievementGroup; }



    public void SaveSettingsToStorage()
    {
        List<PoleGameSetting> settings = new List<PoleGameSetting>();
        settings.Add(new PoleGameSetting("JacketBar1", nJBar[0]));
        settings.Add(new PoleGameSetting("JacketBar2", nJBar[1]));
        settings.Add(new PoleGameSetting("JacketBar3", nJBar[2]));
        settings.Add(new PoleGameSetting("JacketBar4", nJBar[3]));

        // Store the Option Button conditions
        settings.Add(new PoleGameSetting("OptionButton0", nFroshLameness));
        settings.Add(new PoleGameSetting("OptionButton1", nSound));
        settings.Add(new PoleGameSetting("OptionButton2", nEnhancedGraphics));
        settings.Add(new PoleGameSetting("OptionButton3", nRMBFunction));

        // Store the high scores
        settings.Add(new PoleGameSetting("HighScore0", nHighScore[0]));
        settings.Add(new PoleGameSetting("HighScore1", nHighScore[1]));

        for (int i = 0; i < PoleGameAchievement.List.Count; i++)
        {
            PoleGameAchievement achievement = PoleGameAchievement.List[i];
            settings.Add(new PoleGameSetting("A" + achievement.AchievementGuid.ToString(), achievement.Achieved ? achievement.AchievedCode : 0));
        }
        Globals.GameSettingsPersistance.SaveSettings(settings);


    }


    private int SnagSetting(List<PoleGameSetting> settings, string key, int defaultValue)
    {
        foreach (PoleGameSetting p in settings)
            if (p.Name == key) return p.Value;
        return defaultValue;
    }

    public void LoadSettingsFromStorage()
    {
        List<PoleGameSetting> settings = Globals.GameSettingsPersistance.LoadSettings();
        SetJBar(0, SnagSetting(settings, "JacketBar1", -1));
        SetJBar(1, SnagSetting(settings, "JacketBar2", -1));
        SetJBar(2, SnagSetting(settings, "JacketBar3", -1));
        SetJBar(3, SnagSetting(settings, "JacketBar4", -1));
        SetHighScore(0, SnagSetting(settings, "HighScore0", 0));
        SetHighScore(1, SnagSetting(settings, "HighScore1", 0));
        nFroshLameness = SnagSetting(settings, "OptionButton0", 0);
        nSound = SnagSetting(settings, "OptionButton1", 1);
        nEnhancedGraphics = SnagSetting(settings, "OptionButton2", 1);
        nRMBFunction = SnagSetting(settings, "OptionButton3", 1);
        for (int i = 0; i < PoleGameAchievement.List.Count; i++)
        {
            PoleGameAchievement a = PoleGameAchievement.List[i];
            a.Achieved = 
                SnagSetting(settings, "A" + a.AchievementGuid.ToString(), 0) == a.AchievedCode ? true : false;
        }
    }



    public GameConditions()
    {
        nBarGroup = 0;

        for (int i = 0; i < AIMethods.NUM_JBARSPOTS; i++)
            nJBar[i] = AIMethods.NO_BAR;
        Reset(false);
        nFroshLameness = 0;
        nSound = 1;
        nEnhancedGraphics = 0;
        nRMBFunction = 0;

        bRitual = false;

    }

    public void SetHighScore(int nIndex, int nScore)
    {
        nHighScore[nIndex] = nScore;
    }

    public int GetHighScore(int nIndex)
    {
        return nHighScore[nIndex];
    }

    public int[] nStartValue = {	
	1,	30,	80,	0,	0,	0,	6,	0 };
    public int[] nIncrement = {
	1,	30,-10,	1,	1,	1, -1,	1 };
    public int[] nFinalValue = {
	20,1500,50,	1,	1,	1,	2,	1 };
    public int[] nStartTime = {
	0,	0,	0,	3,	4,	5,	5,	7 };

    public void Reset(bool bIsThisADemo)
    {

        bGameOver = false;
        bIsDemo = bIsThisADemo;

        nTopples = 0;
        nNoiseCount = 800;

        for (int j = 0; j < AIMethods.NUM_RINGSPOTS; j++)
            bRingSpot[j] = false;

        ResetEnergy();

        nBoosterClock = -1;
        for (int i = 0; i < AIMethods.NUM_PERFORMACEBOOSTS; i++)
            nBooster[i] = nStartValue[i];

        for (int k = 0; k < AIMethods.NUM_TRICKS; k++)
            bSpecialRingTrick[k] = false;

        bPopBoyInPit = false;
        nPrezHit[0] = nPrezHit[1] = nPrezHit[2] = nPrezHit[3] = nPrezHit[4] = 0;
        if (nFroshLameness == 1)
        {
            nApples = 0;
            nPizza = 5;
            nClark = 0;
            nExam = 0;
            nHose = 0;
            nRing = 0;
        }
        else
        {
            nApples = 5;
            nPizza = 10;
            nClark = 10;
            nExam = 1;
            nHose = 0;
            nRing = 0;
        }
    }

    public void SetJBar(int nBarSpot, int nNewBar)
    {
        nJBar[nBarSpot] = nNewBar;
        bRitual = nJBar[0] == 19 || nJBar[1] == 19
            || nJBar[2] == 19 || nJBar[3] == 19;
    }

    public void PerformanceBoost()
    {
        //	Globals.Debug("The Frosh just got a bit smarter.");
        nBoosterClock++;

        for (int i = 0; i < 8; i++)
        {
            if (nStartTime[i] <= nBoosterClock && nBooster[i] != nFinalValue[i])
            {
                //			Globals.Debug("One characteristic improved.");
                nBooster[i] += nIncrement[i];
            }
        }
    }
}