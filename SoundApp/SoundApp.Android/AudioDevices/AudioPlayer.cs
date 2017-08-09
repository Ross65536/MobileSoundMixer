
using System;
using Android.Media;
using SoundApp;
using SoundApp.Audio;

[assembly: Xamarin.Forms.Dependency(typeof(SoundApp.Droid.AudioDevices.AudioPlayer))]
namespace SoundApp.Droid.AudioDevices
{
    public class AudioPlayer : IAudioPlayerAdapter
    {
        AudioTrack _audioTrack = null;

        public void Play16bitPCMStream(PcmChunk pcmWave)
        {
            Stop();

            ChannelOut channelOut;
            if (pcmWave.NumChannels == 1)
                channelOut = ChannelOut.Mono;
            else if (pcmWave.NumChannels == 2)
                channelOut = ChannelOut.Stereo;
            else
                throw new Exception("number of channels not supported.");

            if (pcmWave.BitDepth != PcmBitDepth.Int16)
                throw new ArgumentException("Unsupported PCM bit depth");

            _audioTrack = new AudioTrack(
            Stream.Music,
            (int)pcmWave.SampleRate,
            channelOut,
            Encoding.Pcm16bit,
            pcmWave.Data.Length,
            AudioTrackMode.Static);

            _audioTrack.Write(pcmWave.Data, 0, pcmWave.Data.Length);
            _audioTrack.Play();
        }

        public void Stop()
        {
            if (_audioTrack == null)
                return;

            if (_audioTrack.PlayState == PlayState.Playing)
                _audioTrack.Pause();   

            _audioTrack.Dispose();
            _audioTrack = null;
        }
    }
}