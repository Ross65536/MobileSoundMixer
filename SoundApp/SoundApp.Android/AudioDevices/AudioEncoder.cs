
using System;
using System.Threading.Tasks;
using Android.Content;
using Java.Lang;
using SoundApp.Audio.AudioWaves;
using SoundApp.Droid.AudioDevices.Decoder;
using SoundApp.PlatformAdapters;
using Exception = System.Exception;

[assembly: Xamarin.Forms.Dependency(typeof(SoundApp.Droid.AudioDevices.AudioEncoder))]
namespace SoundApp.Droid.AudioDevices
{
    public class AudioEncoder : IAudioEncoder
    {
        Task<Android.Net.Uri> fileURITask = null;
        

        public Task<string> PickFileAsync()
        {
            Intent intent = new Intent(Intent.ActionCreateDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("audio/mpeg");

            var listener = new FilePickerListenerAsync(intent);
            fileURITask = listener.URITask;
            return listener.FileNameTask;
        }

        public Task StartEncodingAsync(ISoundWave wave)
        {
            var fileURI = fileURITask.Result;
            if (fileURI == null)
                throw new Exception("Invlaid URI");

            return null;
        }
    }
}