using System;
using Xamarin.Forms;

using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SoundApp.GUI
{
    struct TrackViewUnit
    {
        public String MainName { get; set; }

    }

    public partial class MainPage : ContentPage
    {
        IAudioPlayerAdapter _player = DependencyService.Get<IAudioPlayerAdapter>();
        MusicBuilder _musicBuilder = new MusicBuilder();
        Collection<TrackViewTextItem> _viewListItems;
        public MainPage()
        {
            InitializeComponent();

            this.innitializeFields();
            
            //TODO REMOVE
            TestRuntime();

            
        }

        private void innitializeFields()
        {

            _viewListItems = TrackViewTextItem.SetupListBindingsFactory(this.trackListView);
            this.trackListView.ItemSelected += this.OnTrackSelection;
        }

        private void playButton_Clicked(object sender, EventArgs e)
        {
            var data = _musicBuilder.BuildMusicFacade();

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

            _viewListItems.Add(new TrackViewTextItem ("Yello", track1 ));
            _viewListItems.Add(new TrackViewTextItem ("Yello",track2 ));
            _viewListItems.Add(new TrackViewTextItem ("Yello", track3 ));
        }

        private void stopButton_Clicked(object sender, EventArgs e)
        {
            _player.Stop();
        }

        private void addTrackButton_Clicked(object sender, EventArgs e)
        {
            var track = TrackViewTextItem.DefaultFactory();
            var page = new TrackPage(track);
            page.ChangesSaved += new SaveHandler( this.AddTrack);
            
            Navigation.PushAsync(page);
            
        }

        

        void OnTrackSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var page = new TrackPage((TrackViewTextItem)e.SelectedItem);
            page.ChangesSaved += new SaveHandler(this.UpdateTrack);
            Navigation.PushAsync(page);
            
            ((ListView)sender).SelectedItem = null;
            
        }

        private void AddTrack(TrackViewTextItem oldTrackItem, TrackViewTextItem savedTrackItem)
        {
            this._viewListItems.Add(savedTrackItem);
            this._musicBuilder.AddTrack(savedTrackItem.Track);
        }

        private void UpdateTrack(TrackViewTextItem oldTrackItem, TrackViewTextItem savedTrackItem)
        {
            var index = this._viewListItems.IndexOf(oldTrackItem);
            _viewListItems.RemoveAt(index);
            this._viewListItems.Insert(index, savedTrackItem);

            _musicBuilder.RemoveTrack(oldTrackItem.Track);
            _musicBuilder.AddTrack(savedTrackItem.Track);
        }

        private void clearTracksButton_Clicked(object sender, EventArgs e)
        {
            this._viewListItems.Clear();
            this._musicBuilder.Clear();
        }
    }
}
