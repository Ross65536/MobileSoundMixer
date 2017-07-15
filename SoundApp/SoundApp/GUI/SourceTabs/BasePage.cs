using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;

namespace SoundApp.GUI.SourceTabs
{
    public delegate void SaveWaveHandler (ISoundWave wave);


    public abstract class BasePage : ContentPage
	{
        public event SaveWaveHandler ChangesSaved;

        public BasePage ()
		{
		}

        protected void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        protected void saveButton_Clicked(object sender, EventArgs e)
        {
            var wave = generateSoundWave();
            if (ChangesSaved != null)
                ChangesSaved(wave);

            Navigation.PopAsync();
        }

        abstract protected ISoundWave generateSoundWave();
        
        protected void playSampleButton_Clicked(object sender, EventArgs e)
        {
            var wave = generateSoundWave();
            wave.PlayAudioWave();
        }
        protected void stopButton_Clicked(object sender, EventArgs e)
        {
            AudioStuff.AudioPlayer.Stop();
        }
        
    }
}