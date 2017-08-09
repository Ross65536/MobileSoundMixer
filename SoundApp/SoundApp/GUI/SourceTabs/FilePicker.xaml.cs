using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using SoundApp.PlatformAdapters;

namespace SoundApp.GUI.SourceTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilePicker : BasePage
    {
        private ISoundWave decodedWave = null;
        protected override ISoundWave resultingWave
        { get { return decodedWave; } }

        public FilePicker()
        {
            InitializeComponent();
            AddNavigationButtons(mainStackLayout);

            base.setButtonsValidity(false);
        }

        public void DisplayUIWarningMessage(string text, bool isTextRed)
        {
            infoLabel.Text = text;
            if(isTextRed)
                infoLabel.TextColor = Color.Red;
            else
                infoLabel.TextColor = Color.Default;

            base.setButtonsValidity(false);
        }

        private async void pickLocalFileButton_Clicked(object sender, EventArgs e)
        {
            var fileName = await AudioStuff.AudioDecoder.PickFileAsync();
            if (fileName == null)
                return;
            
            this.fileEntry.Text = fileName;

            DisplayUIWarningMessage("Loading ...", false);
            ISoundWave wave;
            try
            {
                wave = await AudioStuff.AudioDecoder.StartDecodingAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                wave = null;
            }
            

            if (wave == null)
                DisplayUIWarningMessage("Chosen Audio File is not supported OR an error occured.", true);
            else
            {
                DisplayUIWarningMessage("", false);
                decodedWave = wave;
                base.setButtonsValidity(true);
            }
        }
    }
}