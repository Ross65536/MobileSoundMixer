using Android.App;
using Android.Content;
using SoundApp.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SoundApp.Droid.AudioDevices.Decoder
{
    public class FilePickerListenerAsync
    {
        public const int FILE_PICKER_ID = 1;
        public Task<string> FileNameTask { get { return this.CompleteFileName.Task; } }
        public Task<Android.Net.Uri> URITask { get { return this.CompleteURI.Task; } }

        private TaskCompletionSource<Android.Net.Uri> CompleteURI = new TaskCompletionSource<Android.Net.Uri>();
        private TaskCompletionSource<string> CompleteFileName = new TaskCompletionSource<string>();
        private MainActivity mainActivity { get { return ((MainActivity)Forms.Context); } }


        public FilePickerListenerAsync(Intent intent)
        {
            // subscribe to activity results
            mainActivity.ActivityResult += OnActivityResult;
            mainActivity.StartActivityForResult(intent, FilePickerListenerAsync.FILE_PICKER_ID);
        }

        private void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode != FILE_PICKER_ID)
                return;

            // unsubscribe from activity results
            mainActivity.ActivityResult -= OnActivityResult;

            // process result
            if (resultCode == Result.Ok && data != null)
            {
                var uri = data.Data;
                this.CompleteURI.TrySetResult(uri);
                this.CompleteFileName.TrySetResult(uri.LastPathSegment);
            }
            else
            {
                this.CompleteFileName.TrySetResult(null);
                this.CompleteURI.TrySetResult(null);
            }
        }
    }
}