using System;
using System.Threading.Tasks;
using Android.Media;
using SoundApp;
using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.PlatformAdapters;
using Android.Content;
using Android.App;
using Xamarin.Forms;
using SoundApp.Droid.AudioDevices.Decoder;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(SoundApp.Droid.AudioDevices.AudioDecoder))]
namespace SoundApp.Droid.AudioDevices
{
    public class AudioDecoder : IAudioDecoder
    {
        Task<Android.Net.Uri> fileURITask = null;
        AndroidDecoder decoder = null;

        public Task<string> PickFileAsync()
        {
            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("audio/*");

            var listener = new FilePickerListenerAsync(intent);
            fileURITask = listener.URITask;
            return listener.FileNameTask;
        }

        public async Task<ISoundWave> StartDecodingAsync()
        {
            var fileURI = fileURITask.Result;
            if (fileURI == null)
                return null;
            
            try
            {
                decoder = new AndroidDecoder(fileURI);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }

            if (! this.isFileValid(decoder))
                return null;
            
            var wave =  await Task.Run(() => decoder.GetResultingSoundWave() );
            decoder = null;
            return wave;
        }

        private bool isFileValid(AndroidDecoder decoder)
        {
            var sampleRate = decoder.SampleRate;
            var nChannels = decoder.NChannels;

            if (sampleRate != SampleRate.F44_1kHz || nChannels > 2)
                return false;
            else
                return true;
        }

        public void StopDecoding()
        {
            if (decoder != null)
            {
                decoder.ContinueDecoding = false;
                decoder = null;
            }
        }

    }
}