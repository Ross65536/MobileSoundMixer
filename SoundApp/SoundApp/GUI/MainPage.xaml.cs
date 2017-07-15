using System;
using Xamarin.Forms;

using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SoundApp.Audio;

namespace SoundApp.GUI
{
    struct TrackViewUnit
    {
        public String MainName { get; set; }

    }

    public partial class MainPage : ContentPage
    {
        
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
            if (_musicBuilder.IsEmpty)
                return;

            _musicBuilder.BuildMusicFacade().PlayAudioWave();
        }
        

        private void TestRuntime()
        {
            double RUNTIME = 1.0;

            var wave = WaveFactory.MakeWave(WaveTypes.SineWave, new WaveAttributes(Constants.TRACK_SAMPLE_RATE, RUNTIME, 300));
            
            var track1 = new TrackUnit(new EffectsBuilder(wave), 0.0);
            
            _musicBuilder.AddTrack(track1);

            _viewListItems.Add(new TrackViewTextItem ("Yello", track1 ));
            
        }

        private void stopButton_Clicked(object sender, EventArgs e)
        {
            AudioStuff.AudioPlayer.Stop();
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
