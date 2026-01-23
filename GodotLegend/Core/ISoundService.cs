using System;
using System.Collections.Generic;
using System.Text;

public interface ISoundService
{
    void Initialize();

    /// <summary>
    /// Called during game update to allow for periodic updating of audio (for 3D etc.)
    /// </summary>
    void Update();

    bool LoadShortSounds();

    /// <summary>
    /// Returns true when the entire set is loaded
    /// </summary>
    /// <param name="percentageToLoad">The percentage of the total sounds to load 
    /// (allows for rendering of progress meter on splash screen)</param>
    /// <returns></returns>
    bool LoadLongSounds(int percentageToLoad);

    void ShutUp();

}






public interface IStaticSound
{
    void Play(int volume, int pan);
    void Play(int volume);
    void Play();
    void Loop(int volume);
    void Stop();
    bool IsPlaying();
}
public interface IStreamedSound : IStaticSound
{
}




    //public bool Initialize_ShortSounds()
    //{
    //    int i = 0;
    //    int nBuffers;
    //    char[] sFName = new char[MAXFILELINESIZE + 10];
    //    // Open the Sounds Registry file.
    //    FILE* fInput;
    //    if ((fInput = fopen("Sound\\shortsounds.txt", "rt")) == null)
    //        return false;

    //    while (!feof(fInput))
    //    {
    //        fgets(&sFName[0], MAXFILELINESIZE, fInput);
    //        int j;
    //        for (j = 0; j < MAXFILELINESIZE + 10; j++)
    //        {
    //            if (sFName[j] == 10)
    //            {
    //                sFName[j] = '\0';
    //                break;
    //            }
    //        }
    //        if (!(sFName[0] == '/' || sFName[0] == '\0' || feof(fInput)))
    //        {
    //            nBuffers = atoi(&sFName[strlen(sFName) - 2]);
    //            sFName[strlen(sFName) - 2] = '\0';
    //            AIMethods.sSound[i] = new AudioStatic(); //AudioBite
    //            if (!AIMethods.NO_SOUND)
    //                AIMethods.sSound[i].Initialize(sFName, pASServices.GetPDS());
    //            i++;
    //        }
    //    }
    //    // Close the sound registry
    //    fclose(fInput);
    //    return true;
    //}

    //public bool Initialize_LongSounds()
    //{
    //    int i = 0;
    //    char[] sFName = new char[MAXFILELINESIZE + 10];
    //    // Open the Sounds Registry file.
    //    FILE* fInput;
    //    if ((fInput = fopen("Sound\\longsounds.txt", "rt")) == null)
    //        return false;

    //    while (!feof(fInput))
    //    {
    //        fgets(&sFName[0], MAXFILELINESIZE, fInput);
    //        int j;
    //        for (j = 0; j < MAXFILELINESIZE + 10; j++)
    //        {
    //            if (sFName[j] == 10)
    //            {
    //                sFName[j] = '\0';
    //                break;
    //            }
    //        }

    //        if (!(sFName[0] == '/' || sFName[0] == '\0' || feof(fInput)))
    //        {
    //            AIMethods.lSound[i] = new AudioStaticDelayLoad();
    //            if (!AIMethods.NO_SOUND)
    //                AIMethods.lSound[i].Initialize(sFName, pASServices.GetPDS());
    //            i++;
    //            if (0 == (i % 5))
    //                DisplaySplashScreen(i * 100 / NUMBEROFSOUNDS);
    //        }
    //    }
    //    // Close the sound registry
    //    fclose(fInput);
    //    return true;
    //}
