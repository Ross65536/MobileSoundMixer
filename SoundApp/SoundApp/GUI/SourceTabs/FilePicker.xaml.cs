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
        public FilePicker()
        {
            InitializeComponent();
            AddNavigationButtons(mainStackLayout);

            base.setButtonsValidity(false);
        }

        protected override ISoundWave GenerateSoundWave()
        {
            throw new NotImplementedException();
        }

        private async void pickLocalFileButton_Clicked(object sender, EventArgs e)
        {
            var fileName = await AudioStuff.AudioDecoder.PickFile();

            this.fileEntry.Text = fileName;

            AudioStuff.AudioDecoder.StartDecoding();
        }
    }
}