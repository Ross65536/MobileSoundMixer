using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using SoundApp.GUI.SourceTabs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI
{
    public delegate void SaveHandler(TrackViewTextItem oldTrack, TrackViewTextItem savedTrack);
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackPage : ContentPage
    {
        private static int trackNum = 1;

        private TrackViewTextItem modifiedTrack;
        private TrackViewTextItem oldTrack;

        public event SaveHandler ChangesSaved;
        private bool validSave = false;

        public TrackPage(TrackViewTextItem track) 
        {
            InitializeComponent();

            this.oldTrack = track;
            this.modifiedTrack = new TrackViewTextItem(track);

            this.InnitData();

            {
                //TODO: remove
                this.placeHolder();
            }

            
            
        }

        private void InnitData()
        {
            
            if (oldTrack.Track == null)
            {
                this.nameEntry.Text = string.Concat("Track ", trackNum.ToString());
                trackNum++;
                setTrackValidity(false);
            }
            else
            {
                this.nameEntry.Text = modifiedTrack.MainText;
                setTrackValidity(true);
            }

            
        }

        private void setTrackValidity(bool isValid)
        {
            this.saveButton.IsEnabled = isValid;
        }

        private void placeHolder()
        {
            this.effectsListView.ItemsSource = new string[]
            {
                "a",
                "b",
                "c",
                "d"
            };

            {
                double RUNTIME = 1.0;
                var sampleRate = SampleRate.F48kHz;
                WaveChunk wave = WaveFactory.MakeWave(WaveTypes.SineWave, new WaveAttributes(sampleRate, RUNTIME, 300));
                var track1 = new TrackUnit(wave, 0.0);
                modifiedTrack.Track = track1;
            }
        }

        

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            this.saveChangesToTrack();

            if (ChangesSaved != null)
                ChangesSaved(oldTrack, modifiedTrack);
            Navigation.PopAsync();
            
        }

        private void saveChangesToTrack()
        {
            
            this.modifiedTrack.MainText= this.nameEntry.Text;
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void addEffectButton_Clicked(object sender, EventArgs e)
        {
            //TODO
        }

        private void clearEffectsButton_Clicked(object sender, EventArgs e)
        {

        }

        private void pickSourceButton_Clicked(object sender, EventArgs e)
        {
            var page = new SoundSourceTabPage();
            page.ChangesSaved += this.setAudioWaveHandler;
            Navigation.PushAsync(page);
        }

        private void setAudioWaveHandler(WaveChunk wave)
        {
            if (wave == null)
                return;

            var startTime = 0.0;
            modifiedTrack.Track = new TrackUnit(wave, startTime);

            setTrackValidity(true);

        }
    }
}