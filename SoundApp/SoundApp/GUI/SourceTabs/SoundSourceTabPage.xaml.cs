using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SoundApp.PlatformAdapters;
using System.Diagnostics;

namespace SoundApp.GUI.SourceTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoundSourceTabPage : TabbedPage
    {
        public event SaveWaveHandler ChangesSaved;

        public SoundSourceTabPage()
        {
            InitializeComponent();

            addPage(new GeneratorPage());
            var recordedPage = new RecordPage();
            recordedPage.Disappearing += (a, b) => { AudioStuff.AudioRecorder.StopRecording(); };
            addPage(recordedPage);
            addPage(new FilePicker());

        }

        private void addPage(BasePage page)
        {
            SaveWaveHandler eventPipe = (X) => ChangesSaved(X);
            page.ChangesSaved += eventPipe;

            this.Children.Add(page);
        }
         
    }
}