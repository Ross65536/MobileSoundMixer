using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
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
        

        public TrackPage(TrackViewTextItem track) 
        {                                                                                                                                                                                                        
            this.oldTrack = track;
            this.modifiedTrack = new TrackViewTextItem(track);

            InitializeComponent();


            //TODO: remove
            this.placeHolder();

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
                modifiedTrack.MainText = "Newbie";
                modifiedTrack.Track = track1;
            }
        }

        

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (ChangesSaved != null)
                ChangesSaved(oldTrack, modifiedTrack);
            Navigation.PopAsync();
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void addEffectButton_Clicked(object sender, EventArgs e)
        {
            //TODO
        }
    }
}