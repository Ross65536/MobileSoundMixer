using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using SoundApp.GUI.SourceTabs;
using SoundApp.PlatformAdapters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI
{
    public delegate void SaveHandler(TrackViewTextItem oldTrack, TrackViewTextItem savedTrack);
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackPage : ContentPage
    {
        private TrackViewTextItem modifiedTrack;
        private TrackViewTextItem oldTrack;

        public event SaveHandler ChangesSaved;
        private bool startTimeValid = true;

        public TrackPage(TrackViewTextItem track) 
        {
            InitializeComponent();

            this.oldTrack = track;
            this.modifiedTrack = new TrackViewTextItem(track);

            this.checkTrackValidity();
            this.BindingContext = modifiedTrack;
            this.innitGUI();
            
        }

        private void innitGUI()
        {
            this.startTimeSlider.ValueChanged += 
                (sender, e) => { this.startTimeEntry.Text = ((Slider)sender).Value.ToString(); };
            this.startTimeEntry.TextChanged += (sender, e) =>
                { this.onStartTimeEntryHandler((Entry)sender, ref startTimeValid, startTimeSlider); };

        }

        private void onStartTimeEntryHandler(Entry entry, ref bool validEntry, Slider slider)
        {
            bool valid = Double.TryParse(entry.Text, out double result);
            if (valid && result >= 0.0)
            {
                entry.TextColor = Color.Black;
                validEntry = true;
                modifiedTrack.Track.StartTime = result;
                
            }
            else
            {
                entry.TextColor = Color.Red;
                validEntry = false;
                
            }

            checkTrackValidity();

        }

        private void checkTrackValidity()
        {

            if (modifiedTrack.Track.Duration >  0.0 && modifiedTrack.Track.EndTime > 0.0 && this.startTimeValid)
                setButtonValidity(true);
            else
                setButtonValidity(false);
        }

        private void setButtonValidity(bool isValid)
        {
            this.saveButton.IsEnabled = isValid;
            this.playButton.IsEnabled = isValid;
            this.stopButton.IsEnabled = isValid;

        }
        
        private void saveButton_Clicked(object sender, EventArgs e)
        {
            //this.saveChangesToTrack();

            if (ChangesSaved != null)
                ChangesSaved(oldTrack, modifiedTrack);
            Navigation.PopAsync();
            
        }

        private void pickSourceButton_Clicked(object sender, EventArgs e)
        {
            var page = new SoundSourceTabPage();
            page.ChangesSaved += this.setAudioWaveHandler;
            Navigation.PushAsync(page);
        }

        private void setAudioWaveHandler(ISoundWave wave)
        {
            if (wave == null)
                return;

            modifiedTrack.Track.BaseWave = wave.ToReadOnly();

            checkTrackValidity();

        }

        private void playButton_Clicked(object sender, EventArgs e)
        {
            modifiedTrack.Track.PlayAudioWave();
        }

        private void stopButton_Clicked(object sender, EventArgs e)
        {
            AudioStuff.AudioPlayer.Stop();
        }
    }
}