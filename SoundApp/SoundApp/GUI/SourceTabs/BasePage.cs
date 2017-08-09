using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoundApp.Audio.AudioWaves;
using SoundApp.PlatformAdapters;
using Xamarin.Forms;

namespace SoundApp.GUI.SourceTabs
{
    public delegate void SaveWaveHandler (ISoundWave wave);
    
    public abstract class BasePage : ContentPage
	{
        public event SaveWaveHandler ChangesSaved;
        Button playButton;
        Button stopButton;
        Button saveButton;
        abstract protected ISoundWave resultingWave { get; }

        public BasePage ()
		{ }

        protected void AddNavigationButtons(Layout<View> layout)
        {
            var navigationLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };

            stopButton = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Stop"
            };
            stopButton.Clicked += stopButton_Clicked;
            navigationLayout.Children.Add(stopButton);

            playButton = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Play"
            };
            playButton.Clicked += playSampleButton_Clicked;
            navigationLayout.Children.Add(playButton);

            saveButton = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Save"
            };
            saveButton.Clicked += saveButton_Clicked;
            navigationLayout.Children.Add(saveButton);

            layout.Children.Add(navigationLayout);

        }

	    private bool buttonSwitch = true;

        protected void setButtonsValidity(bool v)
        {
            if (buttonSwitch == v)
                return;
            buttonSwitch = !buttonSwitch;

            this.saveButton.IsEnabled = v;
            this.playButton.IsEnabled = v;
            this.stopButton.IsEnabled = v;
        }


        protected void saveButton_Clicked(object sender, EventArgs e)
        {
            if (ChangesSaved != null)
                ChangesSaved(resultingWave);
            
            Navigation.PopAsync();
        }
        
        
        protected void playSampleButton_Clicked(object sender, EventArgs e)
        {
            resultingWave.PlayAudioWave();
        }
        protected void stopButton_Clicked(object sender, EventArgs e)
        {
            AudioStuff.AudioPlayer.Stop();
        }
        
    }
}