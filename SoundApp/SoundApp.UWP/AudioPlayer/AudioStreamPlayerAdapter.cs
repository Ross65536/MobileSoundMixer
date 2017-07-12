using System;
using System.IO;
using SoundApp.UWP.AudioPlayer;
using WAVFileGenerator;
using Windows.Media.Playback;

[assembly: Xamarin.Forms.Dependency(typeof(AudioPlayer))]
namespace SoundApp.UWP.AudioPlayer
{
    public class AudioPlayer : IAudioPlayerAdapter
    {
        MediaPlayer _mediaPlayer = new MediaPlayer();

        public void Play16bitPCMStream(byte[] data, byte nChannels, uint sampleRate)
        {
            var stream = WAVGenerator.GenerateWAVInMemoryStream(data, sampleRate, nChannels);

            _mediaPlayer.SetStreamSource(stream.AsRandomAccessStream());
            _mediaPlayer.Play();
        }

        public void Stop()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Pause();
                
            }
        }
    }
}
