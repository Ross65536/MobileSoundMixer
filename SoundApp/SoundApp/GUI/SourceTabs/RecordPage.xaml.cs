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
        ISoundWave recordedWave = NullSoundWave.Singleton;
        public RecordPage()
        {
            InitializeComponent();
            base.AddNavigationButtons(this.mainStackLayout);

            this.stopRecordingButton.IsEnabled = false;
            base.setButtonsValidity(false);
        }

        override protected ISoundWave GenerateSoundWave()
        {
            return WaveFactory.MakeWave(WaveTypes.SineWave, new WaveAttributes { Runtime = 1.0, TargetFrequency = 300, WaveSampleRate = Constants.TRACK_SAMPLE_RATE });

            return recordedWave;
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