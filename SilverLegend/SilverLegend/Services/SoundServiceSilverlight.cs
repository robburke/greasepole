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
using System.Windows.Resources;

namespace SilverLegend.Services
{
    public class SoundServiceSilverlight : ISoundService
    {

        public void Initialize()
        {
            LoadLongSounds(100);
            LoadShortSounds();
        }

        public void Update()
        {
        }
        private bool m_ShortSoundsLoaded = false;
        private bool m_LongSoundsLoaded = false;

        /// <summary>
        /// Works asynchronously and raises ShortSoundsLoaded event when finished.
        /// </summary>
        public bool LoadShortSounds()
        {
            if (m_ShortSoundsLoaded) return true;
            for (int i = 0; i <= ((int)ASSList.ssndWATER_HOSE); i++)
                AIMethods.sSound[i] = new ResourceSound(shortSoundNames[i]);
            m_ShortSoundsLoaded = true;
            return true;
        }
        /// <summary>
        /// Works asynchronously and raises LongSoundsLoaded event when finished.
        /// </summary>
        public bool LoadLongSounds(int percentageToLoad)
        {
            if (m_LongSoundsLoaded) return true;
            for (int i = 0; i <= ((int)ASLList.lsndSCICONM_PopUp3); i++)
                AIMethods.lSound[i] = new ResourceSound(longSoundNames[i]);
            m_LongSoundsLoaded = true;
            return true;
        }

        public void ShutUp()
        {
            for (int i = 0; i <= ((int)ASLList.lsndSCICONM_PopUp3); i++)
                AIMethods.lSound[i].Stop();
            for (int i = 0; i <= ((int)ASSList.ssndWATER_HOSE); i++)
                AIMethods.sSound[i].Stop();
        }

        public void SetMute(bool muted)
        {
            for (int i = 0; i <= ((int)ASLList.lsndSCICONM_PopUp3); i++)
                ((ResourceSound)AIMethods.lSound[i]).SetMute(muted);
            for (int i = 0; i <= ((int)ASSList.ssndWATER_HOSE); i++)
                ((ResourceSound)AIMethods.sSound[i]).SetMute(muted);
        }

        public string[] shortSoundNames = new string[] { 
            "EFFECTS_ACHIEVEMENTUNLOCKED", "EFFECTS_ACHIEVEMENTUNLOCKED2", "EFFECTS_CROWDMURMUR", "EFFECTS_CROWDROAR1", "EFFECTS_CROWDROAR2", "EFFECTS_BIGJACKETWHOOSH", "EFFECTS_BIGJACKETSLAM", "EFFECTS_CHUG", "EFFECTS_CHUGLASTDROP", "EFFECTS_ICONIN", "EFFECTS_ICONOUT", "EFFECTS_KABOOM", "EFFECTS_PIZZAEAT", "EFFECTS_PIZZAREADY", "EFFECTS_POUR", "EFFECTS_PUNCH", "EFFECTS_PUSH1", "EFFECTS_SMACK1", "EFFECTS_SMACK2", "EFFECTS_SNATCH1", "EFFECTS_SPLATTER", "EFFECTS_TOPPLE", "EFFECTS_TOSSBEER", "EFFECTS_TOSSPIZZA", "EFFECTS_WHOOSH1", "EFFECTS_WHOOSH2", "MENU_DECORATEREPEAT", "MENU_DROP", "MENU_GAMEINIT", "MENU_LOADREPEAT", "MENU_PLACEBAR", "MENU_SELECT", "MENU_TITLEREPEAT", "MENU_TOGGLE", "WATER_DRIP", "WATER_RIPPLE", "WATER_SPLASHBIG", "WATER_SPLASHMID", "WATER_SPLASHSMALL", "WATER_HOSE" };

        public string[] longSoundNames = new string[] { 
            "APPLES_OFFER1", "APPLES_OFFER2", "APPLES_OFFER3", "APPLES_OFFER4", "APPLES_OFFER5", "APPLES_OFFER6", "APPLES_OFFER7", "APPLES_OFFER8", "APPLES_OFFER9", "APPLES_OFFER10", "APPLES_OFFERR1", "APPLES_OFFERR2", "ARTSCI_FEMALE_HIT1", "ARTSCI_FEMALE_HIT2", "ARTSCI_FEMALE_HIT3", "ARTSCI_FEMALE_HIT4", "ARTSCI_FEMALE_HIT5", "ARTSCI_FEMALE_PUSH1", "ARTSCI_FEMALE_TAUNT1", "ARTSCI_FEMALE_TAUNT2", "ARTSCI_FEMALE_TAUNT3", "ARTSCI_FEMALE_TAUNT4", "ARTSCI_FEMALE_TAUNT5", "ARTSCI_FEMALE_TAUNT6", "ARTSCI_MALE_HIT1", "ARTSCI_MALE_HIT2", "ARTSCI_MALE_HIT3", "ARTSCI_MALE_HIT4", "ARTSCI_MALE_HIT5", "ARTSCI_MALE_HITR1", "ARTSCI_MALE_HITR2", "ARTSCI_MALE_HITR3", "ARTSCI_MALE_PUSH1", "ARTSCI_MALE_PUSH2", "ARTSCI_MALE_PUSH3", "ARTSCI_MALE_PUSH4", "ARTSCI_MALE_TAUNT1", "ARTSCI_MALE_TAUNT2", "ARTSCI_MALE_TAUNT3", "ARTSCI_MALE_TAUNT4", "ARTSCI_MALE_TAUNT5", "ARTSCI_MALE_TAUNT6", "ARTSCI_MALE_TAUNT7", "ARTSCI_MALE_TAUNTR1", "CLARK_OFFER1", "CLARK_OFFER2", "CLARK_OFFER3", "CLARK_OFFER4", "CLARK_OFFER5", "CLARK_OFFER6", "CLARK_OFFERR1", "CLARK_OFFERR2", "CLARK_OFFERR3", "CLARK_OFFERR4", "COMMIE_FEMALE_HIT1", "COMMIE_FEMALE_HIT2", "COMMIE_FEMALE_HIT3", "COMMIE_FEMALE_HIT4", "COMMIE_FEMALE_PUSH1", "COMMIE_FEMALE_TAUNT1", "COMMIE_FEMALE_TAUNT2", "COMMIE_FEMALE_TAUNT3", "COMMIE_FEMALE_TAUNT4", "COMMIE_MALE_HIT1", "COMMIE_MALE_HIT2", "COMMIE_MALE_HIT3", "COMMIE_MALE_HIT4", "COMMIE_MALE_HIT5", "COMMIE_MALE_HIT6", "COMMIE_MALE_HIT7", "COMMIE_MALE_PHONE1", "COMMIE_MALE_PHONE2", "COMMIE_MALE_PUSH1", "COMMIE_MALE_PUSH2", "COMMIE_MALE_PUSH3", "COMMIE_MALE_PUSH4", "COMMIE_MALE_TAUNT1", "COMMIE_MALE_TAUNT2", "COMMIE_MALE_TAUNTR1", "COMMIE_MALE_TAUNTR2", "COMMIE_MALE_TAUNTR3", "DISCIPLINES_APPLE", "DISCIPLINES_CHEM", "DISCIPLINES_CIVIL", "DISCIPLINES_DEFAULT", "DISCIPLINES_ELEC", "DISCIPLINES_ENGCHEM", "DISCIPLINES_ENGPHYS", "DISCIPLINES_GEO", "DISCIPLINES_MECH", "DISCIPLINES_METALS", "DISCIPLINES_MINING", "DISCIPLINES_RITUAL", "EXAM_OFFER1", "EXAM_OFFER2", "EXAM_OFFER3", "EXAM_OFFER4", "EXAM_OFFER5", "EXAM_TOSS1", "FRECS_BOO1", "FRECS_BOO2", "FRECS_BOO3", "FRECS_CHANT1", "FRECS_CHANTR1", "FRECS_CHANTR2", "FRECS_CHEER1", "FRECS_HITAPPLE1", "FRECS_HITAPPLE2", "FRECS_HITAPPLE3", "FRECS_HITEXAM1", "FRECS_HOWHIGHTHEPOLE", "FRECS_PROGRESS1", "FRECS_PROGRESS2", "FRECS_PROGRESS3", "FRECS_PROGRESS4", "FRECS_PROGRESSR1", "FRECS_REWARD1", "FRECS_REWARD2", "FRECS_REWARD3", "FRECS_REWARD4", "FRECS_REWARD5", "FRECS_REWARD6", "FRECS_REWARDR1", "FRECS_SLAM", "FRECS_STEPHTOP1", "FRECS_STEPHTOP2", "FROSH_APPLEHIT1", "FROSH_APPLEHIT2", "FROSH_CLARKFINISH1", "FROSH_CLARKFINISH2", "FROSH_CLARKFINISH3", "FROSH_ALPRAISE1", "FROSH_ALPRAISE2", "FROSH_ATTOP1", "FROSH_ATTOP2", "FROSH_ATTOP3", "FROSH_ATTOP4", "FROSH_ATTOP5", "FROSH_GOTTAM1", "FROSH_GOTTAM2", "FROSH_GOTTAM3", "FROSH_SHEEP1", "FROSH_SHEEP2", "FROSH_COW1", "FROSH_COW2", "HOSE_OFFER1", "HOSE_OFFER2", "HOSE_OFFER3", "HOSE_OFFERR1", "HOSE_TAKE1", "HOSE_TAKE2", "MUSIC_SCOTLAND", "MUSIC_TITLEINIT", "NARRATOR_CONGRATS", "NARRATOR_GRAPHICSWARN", "NARRATOR_JACKETINIT", "NARRATOR_OPTIONINIT", "NARRATOR_RITUALWARN", "NARRATOR_STARTDELAY2", "NARRATOR_STARTDELAY1", "PIZZA_OFFER1", "PIZZA_OFFER2", "PIZZA_OFFER3", "PIZZA_OFFERR1", "POPBOY_ADVICE1", "POPBOY_ADVICE2", "POPBOY_ADVICE3", "POPBOY_ADVICE4", "POPBOY_ADVICE5", "POPBOY_ADVICE6", "POPBOY_APPLE1", "POPBOY_APPLER1", "POPBOY_BEER1", "POPBOY_BEERR2", "POPBOY_BEERR3", "POPBOY_CHEER1", "POPBOY_CHEER2", "POPBOY_EXAM1", "POPBOY_EXAM2", "POPBOY_EXAM3", "POPBOY_GOTTAM1", "POPBOY_GREETING1", "POPBOY_GREETING2", "POPBOY_HIPPO1", "POPBOY_HIPPOR1", "POPBOY_PIZZA1", "POPBOY_PIZZA2", "PREZ_HITAPPLE1", "PREZ_HITAPPLE2", "PREZ_HITAPPLE3", "PREZ_HITAPPLE4", "PREZ_HITAPPLER4", "PREZ_HITAPPLE5", "PREZ_HITCLARK1", "PREZ_HITCLARK2", "PREZ_HITCLARK3", "PREZ_HITEXAM", "PREZ_HITHOSE1", "PREZ_HITHOSE2", "PREZ_HITHOSER2", "PREZ_HITHOSER3", "PREZ_HITHOSER4", "PREZ_HITPIZZA1", "PREZ_HITPIZZA2", "PREZ_HITPIZZAR2", "PREZ_ENCOURAGE1", "PREZ_ENCOURAGE2", "PREZ_ENCOURAGE3", "PREZ_ENCOURAGE4", "PREZ_ENCOURAGE5", "PREZ_POPBOY1_1", "PREZ_POPBOY1_2", "PREZ_POPBOY1_3", "PREZ_POPBOY2_1", "PREZ_POPBOY2_2", "RING_SWING", "RING_DING", "RING_PRESS", "RING_RISE", "RING_ZAP1", "RING_ZAP2", "RING_ZAP3", "SCICON_FEMALE_HITAPPLES", "SCICON_FEMALE_HITBEER", "SCICON_FEMALE_HITEXAM", "SCICON_FEMALE_HITMISC", "SCICON_FEMALE_HITPIZZA", "SCICON_FEMALE_POPUP1", "SCICON_FEMALE_POPUP2", "SCICON_FEMALE_POPUP3", "SCICON_FEMALE_POPUP4", "SCICON_MALE_HITAPPLES", "SCICON_MALE_HITBEER", "SCICON_MALE_HITEXAM", "SCICON_MALE_HITMISC", "SCICON_MALE_HITPIZZA", "SCICON_MALE_POPUP1", "SCICON_MALE_POPUP2", "SCICON_MALE_POPUP3" };


    }

    public class ResourceSound : IStaticSound, IStreamedSound
    {
        public ResourceSound(string resourceSuffix)
        {
            ResourceSuffix = resourceSuffix;
        }
        private string ResourceSuffix;
        private Uri Uri = null;
        List<MediaElement> MyMediaElements = new List<MediaElement>(); 


        public void Play(int volume, int pan)
        {
            DoPlay(volume, pan, false);
        }

        private static List<MediaElement> MediaElements = null;
        private static int MaxSimultaneousSounds = 14;
        private static MediaElement GetFreeMediaElement()
        {
            if (MediaElements == null)
            {
                MediaElements = new List<MediaElement>();
                for (int i = 0; i < MaxSimultaneousSounds; i++)
                {
                    MediaElement me = new MediaElement();
                    MediaElements.Add(me);
                    Page.Instance.MusicCanvasRoot.Children.Add(me);
                }
            }
            foreach (MediaElement me in MediaElements)
            {
                if (me.Tag == null) return me;
            }
            return null;
        }

        private static readonly string MenuAssemblyPrefix = "/SilverLegendAssetsMenu";
        private static readonly string GameAssemblyPrefix = "/SilverLegendAssetsGame";
        
        private void DoPlay(int volume, int pan, bool looping)
        {
            //return;
            MediaElement mediaElement = GetFreeMediaElement();
            if (mediaElement == null) return;
            mediaElement.MediaEnded += me_MediaEnded;
            mediaElement.Volume = Page.Instance.IsMuted ? 0d : 1d;
            if (looping) mediaElement.Tag = "PLAYLOOP"; else mediaElement.Tag = "PLAYONCE";
            if (this.Uri == null)
            {
                FindSound();
            }
            if (this.Uri == null)
            {
                // uh oh - we still can't find the sound. We need to give up.
                return;
            }
            if (mediaElement.Source != this.Uri)
            {
                mediaElement.Source = this.Uri;
            }
            // TODO: Set Volume and Pan.
            MyMediaElements.Add(mediaElement);
            mediaElement.Position = TimeSpan.Zero;
            mediaElement.Play();
            Globals.Debug("Playing media: " + this.Uri);
        }
        private string MenuUriPath
        {
            get
            {
                return MenuAssemblyPrefix + ";component/Sound/" + ResourceSuffix + ".mp3";
            }
        }
        private string GameUriPath
        {
            get
            {
                return GameAssemblyPrefix + ";component/Sound/" + ResourceSuffix + ".mp3";
            }
        }
        private void FindSound()
        {
            // Try to find it in AssetsMenu.
            StreamResourceInfo sriMenu = Application.GetResourceStream(new Uri(MenuUriPath, UriKind.Relative));
            if (sriMenu != null)
            {
                this.Uri = new Uri(MenuUriPath, UriKind.Relative);
                return;
            }
            StreamResourceInfo sriGame = Application.GetResourceStream(new Uri(GameUriPath, UriKind.Relative));
            if (sriGame != null)
            {
                this.Uri = new Uri(GameUriPath, UriKind.Relative);
                return;
            }
            return;
        }

        private static readonly int DefaultVolume = 100;
        private static readonly int DefaultPan = 0;

        void me_MediaEnded(object sender, RoutedEventArgs e)
        {
            Globals.Debug("Media Ended: " + this.Uri);
            MediaElement me = sender as MediaElement;
            if (((string)me.Tag) == "PLAYLOOP")
            {
                me.Position = TimeSpan.Zero;
                me.Play();
                return;
            }
            else
            {
                ReleaseMediaElement(me);
            }
        }

        private void ReleaseMediaElement(MediaElement me)
        {
            me.MediaEnded -= me_MediaEnded;
            me.Stop();
            me.Tag = null; // free it up for someone else to use as a sound.
            MyMediaElements.Remove(me);
        }

        public void Play(int volume) { Play(volume, DefaultPan); }
        public void Play() { Play(DefaultVolume, DefaultPan); }

        public void Loop(int volume)
        {
            DoPlay(volume, DefaultPan, true);
        }

        public void Stop()
        {
            while (MyMediaElements.Count > 0)
                ReleaseMediaElement(MyMediaElements[0]);
        }

        public bool IsPlaying()
        {
            return MyMediaElements.Count > 0;
        }


        internal void SetMute(bool muted)
        {
            foreach (MediaElement me in MyMediaElements)
            {
                me.Volume = muted ? 0d : 1d;
            }
        }
    }

}
