using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI.SourceTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneratorPage : BasePage
    {
        private static readonly WaveTypes[] pickerIndexToWaveType = { WaveTypes.SineWave, WaveTypes.Square, WaveTypes.Sawtooth, WaveTypes.Triangle };

        public GeneratorPage()
        {
            InitializeComponent();

            this.waveTypePicker.SelectedIndex = 0;
        }

        protected override WaveChunk generateSoundWave()
        {
            WaveAttributes waveAttr = new WaveAttributes(Constants.TRACK_SAMPLE_RATE, this.DurationSlider.Value, this.frequencySlider.Value);
            var waveType = pickerIndexToWaveType[this.waveTypePicker.SelectedIndex];
            var wave = WaveFactory.MakeWave(waveType, waveAttr);

            return wave;
        }
    }
}