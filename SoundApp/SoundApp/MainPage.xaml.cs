using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using SoundApp.SoundWaves;
using System.IO;


namespace SoundApp
{
    public partial class MainPage : ContentPage
    {
        IAudioPlayer _player = DependencyService.Get<IAudioPlayer>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void playButton_Clicked(object sender, EventArgs e)
        {
            var sampleRate = SampleRate.F48kHz;
            WaveChunk wave = WaveFactory.MakeWave(WaveTypes.SineWave, new WaveAttributes(sampleRate, 5, 300));

            var buffer = WaveChunk.GetPCM16BitArray(wave);
            

            _player.Play16bitPCMStream(buffer, 1, (uint)wave.SampleRate);


        }

        private void stopButton_Clicked(object sender, EventArgs e)
        {
            _player.Stop();
        }
    }
}
