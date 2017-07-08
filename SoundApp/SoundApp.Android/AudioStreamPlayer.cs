
using System;
using Android.Media;
using SoundApp;
using SoundApp.Droid.AudioPlayer;

[assembly: Xamarin.Forms.Dependency(typeof(AudioStreamPlayer))]
namespace SoundApp.Droid.AudioPlayer
{
    public class AudioStreamPlayer : IAudioPlayer
    {
        AudioTrack _audioTrack = null;

        public void Play16bitPCMStream(byte[] data, byte nChannels, uint sampleRate)
        {
            if (_audioTrack == null)
            {
                ChannelOut channelOut;
                if (nChannels == 1)
                    channelOut = ChannelOut.Mono;
                else if (nChannels == 2)
                    channelOut = ChannelOut.Stereo;
                else
                    throw new Exception("number of channels not supported.");

                _audioTrack = new AudioTrack(
                Stream.Music,
                (int)sampleRate,
                channelOut,
                Encoding.Pcm16bit,
                data.Length,
                AudioTrackMode.Stream);

                //audioTrack.Play();
                _audioTrack.Write(data, 0, data.Length);
                _audioTrack.Play();
            }
        }

        public void Stop()
        {
            if (_audioTrack != null)
            {
                _audioTrack.Pause();
                _audioTrack.Dispose();
                _audioTrack = null;
            }
        }
    }
}