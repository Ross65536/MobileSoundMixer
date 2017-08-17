using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EncoderPage : ContentPage
    {
        private Func<ISoundWave> obtainWave;
        public EncoderPage(Func<ISoundWave> getWave)
        {
            this.obtainWave = getWave;
            InitializeComponent();
            this.saveButton.IsEnabled = false;
        }


        private async void pickFileLocationButton_Clicked(object sender, EventArgs e)
        {
            var fileName = await AudioStuff.AudioEncoder.PickFileAsync();
            if (fileName == null)
                return;

            this.fileEntry.Text = fileName;
            this.saveButton.IsEnabled = true;
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            DisplayUIWarningMessage("Exporting ...", false);

            var wave = obtainWave();
            try
            {
                await AudioStuff.AudioEncoder.StartEncodingAsync(wave);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                DisplayUIWarningMessage("Exporting Failed", true);
                return;
            }

            DisplayUIWarningMessage("Exporting Finished!", false);

        }

        public void DisplayUIWarningMessage(string text, bool isTextRed)
        {
            infoLabel.Text = text;
            if (isTextRed)
                infoLabel.TextColor = Color.Red;
            else
                infoLabel.TextColor = Color.Default;
        }
        
    }
}