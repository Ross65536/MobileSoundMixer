
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

        public void Play16bitPCMStream(PCMChunk pcmWave)
        {
            Stop();

            ChannelOut channelOut;
            if (pcmWave.nChannels == 1)
                channelOut = ChannelOut.Mono;
            else if (pcmWave.nChannels == 2)
                channelOut = ChannelOut.Stereo;
            else
                throw new Exception("number of channels not supported.");

            _audioTrack = new AudioTrack(
            Stream.Music,
            (int)pcmWave.sampleRate,
            channelOut,
            Encoding.Pcm16bit,
            pcmWave.data.Length,
            AudioTrackMode.Static);

            _audioTrack.Write(pcmWave.data, 0, pcmWave.data.Length);
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