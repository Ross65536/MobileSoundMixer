using System;
using System.IO;
using SoundApp.Audio;
using SoundApp.UWP.AudioPlayer;
using WAVFileGenerator;
using Windows.Media.Playback;
using Windows.Media.Core;

[assembly: Xamarin.Forms.Dependency(typeof(AudioPlayer))]
namespace SoundApp.UWP.AudioPlayer
{
    public class AudioPlayer : IAudioPlayerAdapter
    {
        MediaPlayer _mediaPlayer = new MediaPlayer();

        public void Play16bitPCMStream(PCMChunk pcmWave)
        {
            var stream = WAVGenerator.GenerateWAVInMemoryStream(pcmWave);

            var mediaElement= MediaSource.CreateFromStream(stream.AsRandomAccessStream(), "audio");
            _mediaPlayer.Source = mediaElement;
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
