using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;

namespace SoundApp.GUI
{
    static class AudioStuff
    {
        static public IAudioPlayerAdapter AudioPlayer = DependencyService.Get<IAudioPlayerAdapter>();

        static public void PlayAudioChunk(this WaveChunk wave)
        {
            wave.LinearVolumeNormalize();
            var data = wave.ConvertToPCM16BitArray();
            AudioPlayer.Play16bitPCMStream(data, 1, (uint) wave.SampleRate);
        }
    }
}
