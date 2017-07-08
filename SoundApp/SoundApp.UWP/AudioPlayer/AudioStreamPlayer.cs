using System;
using System.IO;
using SoundApp.UWP.AudioPlayer;
using WAVFileGenerator;
using Windows.Media.Playback;

[assembly: Xamarin.Forms.Dependency(typeof(AudioPlayer))]
namespace SoundApp.UWP.AudioPlayer
{
    public class AudioPlayer : IAudioPlayer
    {
        MediaPlayer _mediaPlayer = null;

        public void Play16bitPCMStream(byte[] data, byte nChannels, uint sampleRate)
        {
            if (_mediaPlayer == null)
            {
                var stream = WAVGenerator.GenerateWAVInMemoryStream(data, sampleRate, nChannels);
                _mediaPlayer = new MediaPlayer();

                _mediaPlayer.SetStreamSource(stream.AsRandomAccessStream());
                _mediaPlayer.Play();
            }
        }

        public void Stop()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Pause();
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }
        }
    }
}
