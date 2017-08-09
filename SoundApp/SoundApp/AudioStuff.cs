using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using SoundApp.PlatformAdapters;
using Xamarin.Forms;

namespace SoundApp
{
    public static class AudioStuff
    {
        static public IAudioPlayerAdapter AudioPlayer = DependencyService.Get<IAudioPlayerAdapter>();
        static public IAudioRecorderAdapter AudioRecorder = DependencyService.Get<IAudioRecorderAdapter>();
        static public IAudioDecoder AudioDecoder = DependencyService.Get<IAudioDecoder>();


        static public void PlayAudioWave(this IReadOnlyAudioWave wave)
        {
            var waveData = wave.ToPCMTemplate(PcmBitDepth.Int16);
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

        public static SampleRates TargetSampleRate
        {
            get { return SampleRates.F44_1KHz; }
        }

        public static byte TargetPlayingNCHannels { get { return 2; } }
    }
}
