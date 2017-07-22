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
        private bool durationValid = true, freqValid = true;

        public GeneratorPage()
        {
            InitializeComponent();
            InnitGUI();

            AddNavigationButtons(mainStackLayout);
        }

        private void InnitGUI()
        {
            this.waveTypePicker.SelectedIndex = 0;
            this.frequencySlider.ValueChanged += (sender, e) =>
                { frequencyEntry.Text = ((Slider)sender).Value.ToString(); };
            this.DurationSlider.ValueChanged += (sender, e) =>
            { durationEntry.Text = ((Slider)sender).Value.ToString(); };
            this.durationEntry.TextChanged += (sender, e) =>
            { this.checkEntryValidity(((Entry)sender), ref durationValid); };
            this.frequencyEntry.TextChanged += (sender, e) =>
            { this.checkEntryValidity(((Entry)sender), ref freqValid); };
        }

        private void checkEntryValidity(Entry entry, ref bool validEntry)
        {
            bool valid = Double.TryParse(entry.Text, out double result);
            if (valid && result > 0.0)
            {
                entry.TextColor = Color.Black;
                validEntry = true;
            }
            else
            {
                entry.TextColor = Color.Red;
                validEntry = false;
            }

            this.setButtonsValidity(durationValid && freqValid);

        }

        

        protected override ISoundWave GenerateSoundWave()
        {
            var dur = Double.Parse(this.durationEntry.Text);
            var freq = Double.Parse(this.frequencyEntry.Text);
            var waveAttr = new WaveAttributes(Constants.TRACK_SAMPLE_RATE, dur, freq);
            var waveType = pickerIndexToWaveType[this.waveTypePicker.SelectedIndex];
            var wave = WaveFactory.MakeWave(waveType, waveAttr);

            return wave;
        }
    }
}