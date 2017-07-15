using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;

namespace SoundApp.GUI
{
    static class AudioStuff
    {
        static public IAudioPlayerAdapter AudioPlayer = DependencyService.Get<IAudioPlayerAdapter>();

        static public void PlayAudioWave(this ISoundWave wave)
        {
            var waveData = wave.ToPCM(PCMBitDepth.int16);
            var pcmWave = new PCMChunk { bitDepth = PCMBitDepth.int16, data = waveData.data, nChannels = waveData.nChannels, sampleRate = waveData.sampleRate };
            AudioPlayer.Play16bitPCMStream(pcmWave);
        }
    }
}
