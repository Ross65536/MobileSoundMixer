using Android.App;
using Android.Content;
using Android.Content.PM; 
using Android.OS;
using SoundApp.GUI;
using System;

namespace SoundApp.Droid
{
    [Activity(Label = "SoundApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }


        public event Action<int, Result, Intent> ActivityResult;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (ActivityResult != null)
                this.ActivityResult(requestCode, resultCode, data);
        }
    }
}

