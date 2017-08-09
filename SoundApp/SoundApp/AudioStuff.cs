using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using Xamarin.Forms;

namespace SoundApp.PlatformAdapters
{
    public static class AudioStuff
    {
        static public IAudioPlayerAdapter AudioPlayer = DependencyService.Get<IAudioPlayerAdapter>();
        static public IAudioRecorderAdapter AudioRecorder = DependencyService.Get<IAudioRecorderAdapter>();
        static public IAudioDecoder AudioDecoder = DependencyService.Get<IAudioDecoder>();


        static public void PlayAudioWave(this ReadOnlyPCMAbstract wave)
        {
            var waveData = wave.ToPCM(PCMBitDepth.int16);
            AudioPlayer.Play16bitPCMStream(waveData);
        }

        static public void PlayAudioWave(this ISoundWave wave)
        {
            wave.ToReadOnly().PlayAudioWave();
        }

        static public void PlayAudioWave(this ITrackUnit track)
        {
            track.BaseWave.PlayAudioWave();
        }

        public static SampleRate TargetSampleRate
        {
            get { return SampleRate.F44_1kHz; }
        }

        public static byte TargetPlayingNCHannels { get { return 2; } }
    }
}
