using SoundApp.Audio.AudioWaves;
using SoundApp.PlatformAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI.SourceTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : BasePage
    {
        ISoundWave recordedWave;
        protected override ISoundWave resultingWave
        {
            get { return recordedWave; }
        }

        public RecordPage()
        {
            InitializeComponent();
            base.AddNavigationButtons(this.mainStackLayout);

            this.stopRecordingButton.IsEnabled = false;
            base.setButtonsValidity(false);
        }
        

        private void startRecordingButton_Clicked(object sender, EventArgs e)
        {
            SetRecordButtons(true);
            AudioStuff.AudioRecorder.StartRecording();
        }

        private void SetRecordButtons(bool isRecording)
        {
            this.stopRecordingButton.IsEnabled = isRecording;
            this.startRecordingButton.IsEnabled = !isRecording;
            base.setButtonsValidity(!isRecording);
        }

        private void stopRecordingButton_Clicked(object sender, EventArgs e)
        {
            SetRecordButtons(false);
            recordedWave = AudioStuff.AudioRecorder.StopRecording();
        }

    }
}