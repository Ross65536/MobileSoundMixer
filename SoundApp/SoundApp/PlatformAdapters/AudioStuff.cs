using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using Xamarin.Forms;

namespace SoundApp.PlatformAdapters
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

        static public void PlayAudioWave(this ITrackUnit track)
        {
            
            track.BaseWave.PlayAudioWave();
        }
    }
}
