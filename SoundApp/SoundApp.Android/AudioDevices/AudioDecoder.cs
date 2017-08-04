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

[assembly: Xamarin.Forms.Dependency(typeof(SoundApp.Droid.AudioDevices.AudioDecoder))]
namespace SoundApp.Droid.AudioDevices
{
    public class AudioDecoder : IAudioDecoder
    {

        static Android.Net.Uri fileURI = null;
        public Task<string> PickFile()
        {
            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("audio/*");
            
            var activity = (MainActivity)Forms.Context;
            var listener = new FilePickerListener(activity);
            
            activity.StartActivityForResult(intent, FilePickerListener.FILE_PICKER_ID);

            return listener.Task;

        }

        public Task<ISoundWave> StartDecoding()
        {
            MediaExtractor extractor = new MediaExtractor();
            extractor.SetDataSource(Forms.Context, fileURI, null); 

            return null;
        }

        public void StopDecoding()
        {
            throw new NotImplementedException();
        }

        private class FilePickerListener
        {
            public const int FILE_PICKER_ID = 1;
            private TaskCompletionSource<string> Complete = new TaskCompletionSource<string>();
            public Task<string> Task { get { return this.Complete.Task; } }

            public FilePickerListener(MainActivity activity)
            {
                // subscribe to activity results
                activity.ActivityResult += OnActivityResult;
            }

            private void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                if (requestCode != FILE_PICKER_ID)
                    return; 

                // unsubscribe from activity results
                var context = Forms.Context;
                var activity = (MainActivity)context;
                activity.ActivityResult -= OnActivityResult;

                // process result
                if (resultCode == Result.Ok && data != null)
                {
                    Android.Net.Uri uri = data.Data;
                    AudioDecoder.fileURI = uri;
                    this.Complete.TrySetResult(uri.LastPathSegment);
                }
                else
                    this.Complete.TrySetResult("");
            }
        }
    }
}