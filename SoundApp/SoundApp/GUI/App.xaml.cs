
using Xamarin.Forms;

namespace SoundApp.GUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var page = new NavigationPage(new MainPage());
            //NavigationPage.SetHasNavigationBar(page, false);
            //NavigationPage.SetHasBackButton(page, false);
            //var page = new MainPage();
            MainPage = page;

            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
