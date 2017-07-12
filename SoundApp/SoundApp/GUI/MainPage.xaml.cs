using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Implementations;
using System.IO;

namespace SoundApp
{
    public partial class MainPage : ContentPage
    {
        IAudioPlayerAdapter _player = DependencyService.Get<IAudioPlayerAdapter>();
        MusicBuilder _musicBuilder = new MusicBuilder();

        public MainPage()
        {
            InitializeComponent();

            //TODO REMOVE
            TestRuntime();

            this.trackListView.ItemsSource = new string[]{
              "mono",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis",
              "mono",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis",
              "mono",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis",
              "mono",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis",
              "mono",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis"
            };
        }

        private void playButton_Clicked(object sender, EventArgs e)
        {
            var data = _musicBuilder.BuildMusicFacade();

            //_player.Stop();
            _player.Play16bitPCMStream(data, 1, (uint)_musicBuilder.SampleRate);

              
        }
        

        private void TestRuntime()
        {
            double RUNTIME = 1.0;

            var sampleRate = SampleRate.F48kHz;
            WaveChunk wave = WaveFactory.MakeWave(WaveTypes.SineWave, new WaveAttributes(sampleRate, RUNTIME, 300));
            WaveChunk wave1 = new WaveChunk(wave);
            WaveChunk wave2 = WaveFactory.MakeWave(WaveTypes.Triangle, new WaveAttributes(sampleRate, RUNTIME, 1));
            WaveChunk wave3 = WaveFactory.MakeWave(WaveTypes.SineWave, new WaveAttributes(sampleRate, RUNTIME, 100));
            wave.MultEq(0, wave2);

            var track1 = new TrackUnit(wave, 0.0);
            var track2 = new TrackUnit(wave1, 2 * RUNTIME);
            var track3 = new TrackUnit(wave3, 2 * RUNTIME);

            

            _musicBuilder.AddTrack(track1);
            _musicBuilder.AddTrack(track2);
            _musicBuilder.AddTrack(track3);
        }

        private void stopButton_Clicked(object sender, EventArgs e)
        {
            _player.Stop();
        }

        private void addTrackButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
