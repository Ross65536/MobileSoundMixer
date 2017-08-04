using SoundApp.PlatformAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using SoundApp.UWP.AudioDevices;
using Windows.Storage.Streams;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Core;
using Windows.Media.Playback;
using Windows.Media.Core;

//TODO make it work
[assembly: Xamarin.Forms.Dependency(typeof(AudioRecorder))]
namespace SoundApp.UWP.AudioDevices
{
    class AudioRecorder : IAudioRecorderAdapter
    {


        public void StartRecording()
        {
            //Record();
        }

        public ISoundWave StopRecording()
        {
            //Stop();
            //Play();
            
            return NullSoundWave.Singleton;
        }




        private const string audioFilename = "audio.m4a";
        private string filename;
        private MediaCapture capture;
        private InMemoryRandomAccessStream buffer;

        public static bool Recording;

        private async Task<bool> init()
        {
            if (buffer != null)
            {
                buffer.Dispose();
            }
            buffer = new InMemoryRandomAccessStream();
            if (capture != null)
            {
                capture.Dispose();
            }
            try
            {
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                };
                capture = new MediaCapture();
                await capture.InitializeAsync(settings);
                capture.RecordLimitationExceeded += (MediaCapture sender) =>
                {
                    Stop();
                    throw new Exception("Exceeded Record Limitation");
                };
                capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
                {
                    Recording = false;
                    throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
                {
                    throw ex.InnerException;
                }
                throw;
            }
            return true;
        }

        public async void Record()
        {
            await init();
            await capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto), buffer);
            if (Recording) throw new InvalidOperationException("cannot excute two records at the same time");
            Recording = true;
        }

        public async void Stop()
        {
            await capture.StopRecordAsync();
            Recording = false;
        }

        public void Play()
        {
            IRandomAccessStream audio = buffer.CloneStream();

            MediaPlayer _mediaPlayer = new MediaPlayer();

            var mediaElement = MediaSource.CreateFromStream(audio, "audio");
            _mediaPlayer.Source = mediaElement;
            _mediaPlayer.Play();

            return;

        }
    }
}
