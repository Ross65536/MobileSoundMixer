using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.AudioWaves.Implementations;
using SoundApp.PlatformAdapters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EncoderPage : ContentPage
    {
        private static readonly CompressionOption[] pickerCompressionOptions = { CompressionOption.UncrompressedWAV, CompressionOption.AAC  };
        private const double FILE_SIZE_WARNING_LIMIT = 10.0; //in MiB
        private const int UNCOMPRESSED_PICKER_INDEX = 0;
        private const int PICKER_INITIAL_INDEX = 0;
        private ISoundWave wave;

        public EncoderPage(Func<ISoundWave> getWave)
        {
            wave = getWave();
            InitializeComponent();
            this.saveButton.IsEnabled = false;

            this.encodingTypePicker.SelectedIndexChanged += pickerSelectionChangedHandler;
            this.encodingTypePicker.SelectedIndex = PICKER_INITIAL_INDEX;
        }

        private void pickerSelectionChangedHandler(object sender, EventArgs e)
        {
            if (this.encodingTypePicker.SelectedIndex == UNCOMPRESSED_PICKER_INDEX && uncompressedFileSizeMB > FILE_SIZE_WARNING_LIMIT)
                DisplayUIWarningMessage("Warning: the file will have size ~" + Math.Floor(uncompressedFileSizeMB).ToString() + " MB and It might take a long time to export.", false);
            else
                DisplayUIWarningMessage("", false);
            this.saveButton.IsEnabled = false;
        }

        private double uncompressedFileSizeMB => wave.Duration * 44100 * sizeof(short) * 2 / 1024 / 1024; //2channels

        private async void pickFileLocationButton_Clicked(object sender, EventArgs e)
        {
            var option = pickerCompressionOptions[this.encodingTypePicker.SelectedIndex];
            var fileName = await AudioStuff.AudioEncoder.PickFileAsync(option);
            if (fileName == null)
                return;

            this.fileEntry.Text = fileName;
            this.saveButton.IsEnabled = true;
            
            DisplayUIWarningMessage("",false);
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            DisplayUIWarningMessage("Exporting ...", false);
            
            bool isEncoded;
            try
            {
                isEncoded = await AudioStuff.AudioEncoder.StartEncodingAsync(wave);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                isEncoded = false;
            }

            if(isEncoded)
                DisplayUIWarningMessage("Exporting Finished!", false);
            else
                DisplayUIWarningMessage("Exporting Failed", true);

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