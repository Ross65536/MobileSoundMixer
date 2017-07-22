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
            //var a = Play();
            //a.Wait();
            //return NullSoundWave.Singleton;
            return null;
        }




        //private const string audioFilename = "audio.m4a";
        //private string filename;
        //private MediaCapture capture;
        //private InMemoryRandomAccessStream buffer;

        //public static bool Recording;

        //private async Task<bool> init()
        //{
        //    if (buffer != null)
        //    {
        //        buffer.Dispose();
        //    }
        //    buffer = new InMemoryRandomAccessStream();
        //    if (capture != null)
        //    {
        //        capture.Dispose();
        //    }
        //    try
        //    {
        //        MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
        //        {
        //            StreamingCaptureMode = StreamingCaptureMode.Audio
        //        };
        //        capture = new MediaCapture();
        //        await capture.InitializeAsync(settings);
        //        capture.RecordLimitationExceeded += (MediaCapture sender) =>
        //        {
        //            Stop();
        //            throw new Exception("Exceeded Record Limitation");
        //        };
        //        capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
        //        {
        //            Recording = false;
        //            throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
        //        {
        //            throw ex.InnerException;
        //        }
        //        throw;
        //    }
        //    return true;
        //}

        //public async void Record()
        //{
        //    await init();
        //    await capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto), buffer);
        //    if (Recording) throw new InvalidOperationException("cannot excute two records at the same time");
        //    Recording = true;
        //}

        //public async void Stop()
        //{
        //    await capture.StopRecordAsync();
        //    Recording = false;
        //}

        //public async Task Play()
        //{
        //    MediaElement playback = new MediaElement();
        //    IRandomAccessStream audio = buffer.CloneStream();
        //    if (audio == null) throw new ArgumentNullException("buffer");
        //    StorageFolder storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        //    if (!string.IsNullOrEmpty(filename))
        //    {
        //        StorageFile original = await storageFolder.GetFileAsync(filename);
        //        await original.DeleteAsync();
        //    }


        //        StorageFile storageFile = await storageFolder.CreateFileAsync(audioFilename, CreationCollisionOption.GenerateUniqueName);
        //        filename = storageFile.Name;
        //        using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
        //        {
        //            await RandomAccessStream.CopyAndCloseAsync(audio.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
        //            await audio.FlushAsync();
        //            audio.Dispose();
        //        }
        //        IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);
        //        playback.SetSource(stream, storageFile.FileType);
        //        playback.Play();
        //}
    }
}
