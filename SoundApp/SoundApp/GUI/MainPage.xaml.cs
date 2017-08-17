using System;
using Xamarin.Forms;

using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SoundApp.Audio;
using SoundApp.PlatformAdapters;
using System.Diagnostics;
using SoundApp.Audio.AudioWaves.Implementations;

namespace SoundApp.GUI
{
    struct TrackViewUnit
    {
        public String MainName { get; set; }

    }

    public partial class MainPage : ContentPage
    {
        //TODO fix statics
        static MusicBuilder _musicBuilder = new MusicBuilder(AudioStuff.TargetSampleRate, AudioStuff.TargetPlayingNCHannels);
        static Collection<TrackViewTextItem> _viewListItems;
        public MainPage()
        {
            InitializeComponent();

            this.innitializeFields();
            
            //TODO REMOVE
            TestRuntime();

            
        }

        private void innitializeFields()
        {

            _viewListItems = SetupListBindingsFactory(this.trackListView);
            this.trackListView.ItemSelected += this.OnTrackSelection;
        }

        public delegate void DeleteHandler();

        public class DeletableCell : TextCell
        {
            //public static event 
            public DeletableCell() : base()
            {
                var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; 
                deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                deleteAction.Parent = this;
                deleteAction.Clicked +=  (sender, e) => {
                    var mi = ((MenuItem)sender);
                    var cell = (DeletableCell)mi.Parent;
                    var textItem = (TrackViewTextItem) cell.BindingContext;

                    
                    _musicBuilder.RemoveTrack(textItem.Track);
                    _viewListItems.Remove(textItem);
                    //cell.TextColor = Color.Black;
                    //Debug.WriteLine("Delete Context Action clicked: " + mi.CommandParameter);
                };
                
                ContextActions.Add(deleteAction);
            }
        }

        private static ObservableCollection<TrackViewTextItem> SetupListBindingsFactory(ListView trackListView)
        {
            var list = new ObservableCollection<TrackViewTextItem>();
            
            trackListView.ItemTemplate = new DataTemplate(typeof(DeletableCell));
            trackListView.ItemsSource = list;
            trackListView.ItemTemplate.SetBinding(DeletableCell.TextProperty, "MainText");
            trackListView.ItemTemplate.SetBinding(DeletableCell.DetailProperty, "DetailText");
            trackListView.ItemTemplate.SetBinding(DeletableCell.TextColorProperty, "MainColor");
            return list;
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

            var wave = WaveFactory.MakeClassicWave(WaveTypes.SineWave, new WaveAttributes(AudioStuff.TargetSampleRate, RUNTIME, 300));
            
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
            _viewListItems.Add(savedTrackItem);
            _musicBuilder.AddTrack(savedTrackItem.Track);
        }

        private void UpdateTrack(TrackViewTextItem oldTrackItem, TrackViewTextItem savedTrackItem)
        {
            var index = _viewListItems.IndexOf(oldTrackItem);
            _viewListItems.RemoveAt(index);
            _viewListItems.Insert(index, savedTrackItem);

            _musicBuilder.RemoveTrack(oldTrackItem.Track);
            _musicBuilder.AddTrack(savedTrackItem.Track);
        }

        private void clearTracksButton_Clicked(object sender, EventArgs e)
        {
            _viewListItems.Clear();
            _musicBuilder.Clear();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            var page = new EncoderPage(_musicBuilder.BuildMusicFacade);

            Navigation.PushAsync(page);
        }
    }
}
