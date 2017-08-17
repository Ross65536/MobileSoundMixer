using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.SoundMixer;
using SoundApp.PlatformAdapters;
using Xamarin.Forms;

namespace SoundApp
{
    public static class AudioStuff
    {
        public static IAudioPlayerAdapter AudioPlayer = DependencyService.Get<IAudioPlayerAdapter>();
        public static IAudioRecorderAdapter AudioRecorder = DependencyService.Get<IAudioRecorderAdapter>();
        public static IAudioDecoder AudioDecoder = DependencyService.Get<IAudioDecoder>();
        public static IAudioEncoder AudioEncoder = DependencyService.Get<IAudioEncoder>();


        public static void PlayAudioWave(this IReadOnlyAudioWave wave)
        {
            var waveData = wave.ToPCMTemplate(PcmBitDepth.Int16);
            AudioPlayer.Play16bitPCMStream(waveData);
        }

        public static void PlayAudioWave(this ISoundWave wave)
        {
            wave.ToReadOnly().PlayAudioWave();
        }

        public static void PlayAudioWave(this ITrackUnit track)
        {
            track.BaseWave.PlayAudioWave();
        }

        public static SampleRates TargetSampleRate => SampleRates.F44_1KHz;

        public static byte TargetPlayingNCHannels { get { return 2; } }
    }
}
