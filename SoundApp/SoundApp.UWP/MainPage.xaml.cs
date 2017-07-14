

namespace SoundApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new SoundApp.GUI.App());
        }
    }
}
